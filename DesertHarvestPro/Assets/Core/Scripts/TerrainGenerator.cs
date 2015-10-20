using UnityEngine;
using System.Collections.Generic;


public class TerrainGenerator : MonoBehaviour 
{
	//Prototypes
	public Texture2D m_splat0, m_splat1;
	public float m_splatTileSize0 = 10.0f;
	public float m_splatTileSize1 = 2.0f;
	public Texture2D m_detail0, m_detail1, m_detail2;
    public GameObject solder;
    public GameObject worm;
    public GameObject spice;
    public GameObject[] cliff;
    public GameObject[] water;
    public int cliffSpacing = 40;
    public int spiceSpacing = 120;
    public int waterSpacing = 80;
    public int soldierSpacing = 150;
    public int wormSpacing = 150;
	public GameObject m_tree0, m_tree1, m_tree2;
	//Noise settings. A higher frq will create larger scale details. Each seed value will create a unique look
	public int m_groundSeed = 0;
	public float m_groundFrq = 800.0f;
	public int m_mountainSeed = 1;
	public float  m_mountainFrq = 1200.0f;
	public int m_treeSeed = 2;
	public float  m_treeFrq = 400.0f;
	public int m_detailSeed = 3;
	public float  m_detailFrq = 100.0f;
	//Terrain settings
	public int m_tilesX = 1; //Number of terrain tiles on the x axis
	public int m_tilesZ = 1; //Number of terrain tiles on the z axis
	public float m_pixelMapError = 6.0f; //A lower pixel error will draw terrain at a higher Level of detail but will be slower
	public float m_baseMapDist = 1000.0f; //The distance at which the low res base map will be drawn. Decrease to increase performance
	//Terrain data settings
	public int m_heightMapSize = 513; //Higher number will create more detailed height maps
	public int m_alphaMapSize = 1024; //This is the control map that controls how the splat textures will be blended
	public int m_terrainSize = 2048;
	public int m_terrainHeight = 512;
	public int m_detailMapSize = 512; //Resolutions of detail (Grass) layers
	//Tree settings
	public int m_treeSpacing = 32; //spacing between trees
	public float m_treeDistance = 2000.0f; //The distance at which trees will no longer be drawn
	public float m_treeBillboardDistance = 400.0f; //The distance at which trees meshes will turn into tree billboards
	public float m_treeCrossFadeLength = 20.0f; //As trees turn to billboards there transform is rotated to match the meshes, a higher number will make this transition smoother
	public int m_treeMaximumFullLODCount = 400; //The maximum number of trees that will be drawn in a certain area. 
	//Detail settings
	public DetailRenderMode detailMode;
	public int m_detailObjectDistance = 400; //The distance at which details will no longer be drawn
	public float m_detailObjectDensity = 4.0f; //Creates more dense details within patch
	public int m_detailResolutionPerPatch = 32; //The size of detail patch. A higher number may reduce draw calls as details will be batch in larger patches
	public float m_wavingGrassStrength = 0.4f;
	public float m_wavingGrassAmount = 0.2f;
	public float m_wavingGrassSpeed = 0.4f;
	public Color m_wavingGrassTint = Color.white;
	public Color m_grassHealthyColor = Color.white;
	public Color m_grassDryColor = Color.white;

	//Private
	PerlinNoise m_groundNoise, m_mountainNoise, m_treeNoise, m_detailNoise;
    List<Vector2> takenPositions;
	Terrain[,] m_terrain;
	SplatPrototype[] m_splatPrototypes;
	TreePrototype[] m_treeProtoTypes;
	DetailPrototype[] m_detailProtoTypes;
	Vector2 m_offset;
	
	void Start() 
	{
		m_groundNoise = new PerlinNoise(m_groundSeed);
		m_mountainNoise = new PerlinNoise(m_mountainSeed);
		m_treeNoise = new PerlinNoise(m_treeSeed);
		m_detailNoise = new PerlinNoise(m_detailSeed);
        takenPositions = new List<Vector2>();
		if(!Mathf.IsPowerOfTwo(m_heightMapSize-1))
		{
			Debug.Log("TerrianGenerator::Start - height map size must be pow2+1 number");
			m_heightMapSize = Mathf.ClosestPowerOfTwo(m_heightMapSize)+1;
		}
		
		if(!Mathf.IsPowerOfTwo(m_alphaMapSize))
		{
			Debug.Log("TerrianGenerator::Start - Alpha map size must be pow2 number");
			m_alphaMapSize = Mathf.ClosestPowerOfTwo(m_alphaMapSize);
		}
		
		if(!Mathf.IsPowerOfTwo(m_detailMapSize))
		{
			Debug.Log("TerrianGenerator::Start - Detail map size must be pow2 number");
			m_detailMapSize = Mathf.ClosestPowerOfTwo(m_detailMapSize);
		}
		
		if(m_detailResolutionPerPatch < 8)
		{
			Debug.Log("TerrianGenerator::Start - Detail resolution per patch must be >= 8, changing to 8");
			m_detailResolutionPerPatch = 8;
		}
		
		float[,] htmap = new float[m_heightMapSize,m_heightMapSize];
		
		m_terrain = new Terrain[m_tilesX,m_tilesZ];
		
		//this will center terrain at origin
		m_offset = new Vector2(-m_terrainSize*m_tilesX*0.5f, -m_terrainSize*m_tilesZ*0.5f);
		
		CreateProtoTypes();
		
		for(int x = 0; x < m_tilesX; x++)
		{
			for(int z = 0; z < m_tilesZ; z++)
			{
				FillHeights(htmap, x, z);
				
				TerrainData terrainData = new TerrainData();

				terrainData.heightmapResolution = m_heightMapSize;
				terrainData.SetHeights(0, 0, htmap);
				terrainData.size = new Vector3(m_terrainSize, m_terrainHeight, m_terrainSize);
				terrainData.splatPrototypes = m_splatPrototypes;
				terrainData.treePrototypes = m_treeProtoTypes;
				terrainData.detailPrototypes = m_detailProtoTypes;
                
				
				FillAlphaMap(terrainData);
	
				m_terrain[x,z] = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
				m_terrain[x,z].transform.position = new Vector3(m_terrainSize*x + m_offset.x, 0, m_terrainSize*z + m_offset.y);
				m_terrain[x,z].heightmapPixelError = m_pixelMapError;
				m_terrain[x,z].basemapDistance = m_baseMapDist;
				
				//disable this for better frame rate
				m_terrain[x,z].castShadows = false;
				
				//FillTreeInstances(m_terrain[x,z], x, z);
               
                FillCliffs2(m_terrain[x, z], x, z);
                
                FillCliffs(m_terrain[x, z], x, z);
                FillSpice(m_terrain[x, z], x, z);
                FillWater(m_terrain[x, z], x, z);
                FillSoldierInstances(m_terrain[x, z], x, z);
                FillWormInstances(m_terrain[x, z], x, z);
				//FillDetailMap(m_terrain[x,z], x, z);
			}
		}
		
		//Set the neighbours of terrain to remove seams.
		for(int x = 0; x < m_tilesX; x++)
		{
			for(int z = 0; z < m_tilesZ; z++)
			{
				Terrain right = null;
				Terrain left = null;
				Terrain bottom = null;
				Terrain top = null;
				
				if(x > 0) left = m_terrain[(x-1),z];
				if(x < m_tilesX-1) right = m_terrain[(x+1),z];
				
				if(z > 0) bottom = m_terrain[x,(z-1)];
				if(z < m_tilesZ-1) top = m_terrain[x,(z+1)];
				
				m_terrain[x,z].SetNeighbors(left, top, right, bottom);
	
			}
		}
		
	}
	
	void CreateProtoTypes()
	{
		//Ive hard coded 2 splat prototypes, 3 tree prototypes and 3 detail prototypes.
		//This is a little inflexible way to do it but it made the code simpler and can easly be modified 
		
		m_splatPrototypes = new SplatPrototype[2];
		
		m_splatPrototypes[0] = new SplatPrototype();
		m_splatPrototypes[0].texture = m_splat0;
		m_splatPrototypes[0].tileSize = new Vector2(m_splatTileSize0, m_splatTileSize0);
		
		m_splatPrototypes[1] = new SplatPrototype();
		m_splatPrototypes[1].texture = m_splat1;
		m_splatPrototypes[1].tileSize = new Vector2(m_splatTileSize1, m_splatTileSize1);

        m_treeProtoTypes = new TreePrototype[3];

        m_treeProtoTypes[0] = new TreePrototype();
        m_treeProtoTypes[0].prefab = m_tree0;

        m_treeProtoTypes[1] = new TreePrototype();
        m_treeProtoTypes[1].prefab = m_tree1;

        m_treeProtoTypes[2] = new TreePrototype();
        m_treeProtoTypes[2].prefab = m_tree2;
		
		m_detailProtoTypes = new DetailPrototype[3];

		m_detailProtoTypes[0] = new DetailPrototype();
		m_detailProtoTypes[0].prototypeTexture = m_detail0;
		m_detailProtoTypes[0].renderMode = detailMode;
		m_detailProtoTypes[0].healthyColor = m_grassHealthyColor;
		m_detailProtoTypes[0].dryColor = m_grassDryColor;
		
		m_detailProtoTypes[1] = new DetailPrototype();
		m_detailProtoTypes[1].prototypeTexture = m_detail1;
		m_detailProtoTypes[1].renderMode = detailMode;
		m_detailProtoTypes[1].healthyColor = m_grassHealthyColor;
		m_detailProtoTypes[1].dryColor = m_grassDryColor;
		
		m_detailProtoTypes[2] = new DetailPrototype();
		m_detailProtoTypes[2].prototypeTexture = m_detail2;
		m_detailProtoTypes[2].renderMode = detailMode;
		m_detailProtoTypes[2].healthyColor = m_grassHealthyColor;
		m_detailProtoTypes[2].dryColor = m_grassDryColor;

		
	}
	
	void FillHeights(float[,] htmap, int tileX, int tileZ)
	{
		float ratio = (float)m_terrainSize/(float)m_heightMapSize;
		
		for(int x = 0; x < m_heightMapSize; x++)
		{
			for(int z = 0; z < m_heightMapSize; z++)
			{
				float worldPosX = (x+tileX*(m_heightMapSize-1))*ratio;
				float worldPosZ = (z+tileZ*(m_heightMapSize-1))*ratio;

				float mountains = Mathf.Max(0.0f, m_mountainNoise.FractalNoise2D(worldPosX, worldPosZ, 6, m_mountainFrq, 0.2f));
				
				float plain = m_groundNoise.FractalNoise2D(worldPosX, worldPosZ, 8, m_groundFrq, 0.1f) + 0.1f;
				
				htmap[z,x] = plain + mountains;
			}
		}
	}
	
	void FillAlphaMap(TerrainData terrainData) 
	{
        float[,,] map  = new float[m_alphaMapSize, m_alphaMapSize, 2];
		
		Random.seed = 0;
        
        for(int x = 0; x < m_alphaMapSize; x++) 
		{
            for (int z = 0; z < m_alphaMapSize; z++) 
			{
                // Get the normalized terrain coordinate that
                // corresponds to the the point.
                float normX = x * 1.0f / (m_alphaMapSize - 1);
                float normZ = z * 1.0f / (m_alphaMapSize - 1);
                
                // Get the steepness value at the normalized coordinate.
                float angle = terrainData.GetSteepness(normX, normZ);
                
                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                float frac = angle / 90.0f;
                map[z, x, 0] = frac;
                map[z, x, 1] = 1.0f - frac;
				
            }
        }
        
		terrainData.alphamapResolution = m_alphaMapSize;
        terrainData.SetAlphamaps(0, 0, map);
    }
	
	void FillTreeInstances(Terrain terrain, int tileX, int tileZ)
	{
		Random.seed = 0;
	
		for(int x = 0; x < m_terrainSize; x += m_treeSpacing) 
		{
            for (int z = 0; z < m_terrainSize; z += m_treeSpacing) 
			{
				
				float unit = 1.0f / (m_terrainSize - 1);
				
				float offsetX = Random.value * unit * m_treeSpacing;
				float offsetZ = Random.value * unit * m_treeSpacing;

                float normX = x * unit+offsetX;
                float normZ = z * unit+offsetZ;
                
                // Get the steepness value at the normalized coordinate.
                float angle = terrain.terrainData.GetSteepness(normX, normZ);
                
                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                float frac = angle / 90.0f;
				
				if(frac < 0.5f) //make sure tree are not on steep slopes
				{
					float worldPosX = x+tileX*(m_terrainSize-1);
					float worldPosZ = z+tileZ*(m_terrainSize-1);
					
					float noise = m_treeNoise.FractalNoise2D(worldPosX, worldPosZ, 3, m_treeFrq, 1.0f);
					float ht = terrain.terrainData.GetInterpolatedHeight(normX, normZ);
					
					if(noise > 0.0f && ht < m_terrainHeight*0.4f)
					{
                        int randomTree = Random.Range(0, 3);

                        //GameObject obj = Instantiate(m_treeProtoTypes[randomTree].prefab,
                        //     new Vector3(worldPosX - 1024 + offsetX/unit, ht, worldPosZ - 1024+offsetZ/unit),
                        //     Quaternion.identity) as GameObject;


						TreeInstance temp = new TreeInstance();
						temp.position = new Vector3(normX,ht,normZ);
                        temp.prototypeIndex = randomTree;

                        temp.widthScale =3;
                        temp.heightScale = 10;
                        Debug.Log(temp.widthScale);
						temp.color = Color.blue;
						temp.lightmapColor = Color.white;
						
						terrain.AddTreeInstance(temp);
					}
				}
				
			}
		}
		
		terrain.treeDistance = m_treeDistance;
		terrain.treeBillboardDistance = m_treeBillboardDistance;
		terrain.treeCrossFadeLength = m_treeCrossFadeLength;
		terrain.treeMaximumFullLODCount = m_treeMaximumFullLODCount;
		
	}

    void FillSoldierInstances(Terrain terrain, int tileX, int tileZ)
    {
        Random.seed = 0;
        
       
        for (int x = 0; x < m_terrainSize; x += soldierSpacing)
        {
            for (int z = 0; z < m_terrainSize; z += soldierSpacing)
            {
                float unit = 1.0f / (m_terrainSize - 1);

                float offsetX = Random.value * unit * soldierSpacing;
                float offsetZ = Random.value * unit * soldierSpacing;
               
                float normX = x * unit + offsetX;
                float normZ = z * unit + offsetZ;

                // Get the steepness value at the normalized coordinate.
                float angle = terrain.terrainData.GetSteepness(normX, normZ);

                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                float frac = angle / 90.0f;

                if (frac < 0.5f) //make sure tree are not on steep slopes
                {
                    float worldPosX = x + tileX * (m_terrainSize - 1);
                    float worldPosZ = z + tileZ * (m_terrainSize - 1) ;

                    float noise = m_treeNoise.FractalNoise2D(worldPosX , worldPosZ, 3, m_treeFrq, 1.0f);
                    float ht = terrain.terrainData.GetInterpolatedHeight(normX, normZ);
					
                    if (noise > 0.0f && ht < m_terrainHeight * 0.4f)
                    {
                       
                        GameObject obj = Instantiate(solder,
                             new Vector3(worldPosX - 1024 + offsetX / unit, ht, worldPosZ - 1024 + offsetZ / unit),
                             Quaternion.identity) as GameObject;
                     
                    }
                }
            }
        }

    }

    void FillWormInstances(Terrain terrain, int tileX, int tileZ)
    {
        Random.seed = 6;


        for (int x = 0; x < m_terrainSize; x += wormSpacing)
        {
            for (int z = 0; z < m_terrainSize; z += wormSpacing)
            {
                float unit = 1.0f / (m_terrainSize - 1);

                float offsetX = Random.value * unit * wormSpacing;
                float offsetZ = Random.value * unit * wormSpacing;

                float normX = x * unit + offsetX;
                float normZ = z * unit + offsetZ;

                // Get the steepness value at the normalized coordinate.
                float angle = terrain.terrainData.GetSteepness(normX, normZ);

                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                float frac = angle / 90.0f;

                if (frac < 0.5f) //make sure tree are not on steep slopes
                {
                    float worldPosX = x + tileX * (m_terrainSize - 1);
                    float worldPosZ = z + tileZ * (m_terrainSize - 1);

                    float noise = m_treeNoise.FractalNoise2D(worldPosX, worldPosZ, 3, m_treeFrq, 1.0f);
                    float ht = terrain.terrainData.GetInterpolatedHeight(normX, normZ);

                    if (noise > 0.0f && ht < m_terrainHeight * 0.4f)
                    {

                        GameObject obj = Instantiate(worm,
                             new Vector3(worldPosX - 1024 + offsetX / unit, ht, worldPosZ - 1024 + offsetZ / unit),
                             Quaternion.identity) as GameObject;

                    }
                }
            }
        }

    }
    void FillCliffs(Terrain terrain, int tileX, int tileZ)
    {
        Random.seed = 11;


        for (int x = 0; x < m_terrainSize; x += cliffSpacing)
        {
            for (int z = 0; z < m_terrainSize; z += cliffSpacing)
            {
                //for (int f = 0; f < takenPositions.Count;f++ )
                //{
                //    if (takenPositions[f].x != x && takenPositions[f].y != z)
                //    {
                       // Debug.Log(takenPositions[f].x + " " + takenPositions[f].y);
                        takenPositions.Add(new Vector2(x, z));
                        float unit = 1.0f / (m_terrainSize - 1);

                        float offsetX = Random.value * unit * cliffSpacing;
                        float offsetZ = Random.value * unit * cliffSpacing;

                        float normX = x * unit + offsetX;
                        float normZ = z * unit + offsetZ;

                        // Get the steepness value at the normalized coordinate.
                        float angle = terrain.terrainData.GetSteepness(normX, normZ);

                        // Steepness is given as an angle, 0..90 degrees. Divide
                        // by 90 to get an alpha blending value in the range 0..1.
                        float frac = angle / 90.0f;

                        if (frac < 0.5f) //make sure tree are not on steep slopes
                        {
                            float worldPosX = x + tileX * (m_terrainSize - 1);
                            float worldPosZ = z + tileZ * (m_terrainSize - 1);

                            float noise = m_treeNoise.FractalNoise2D(worldPosX, worldPosZ, 3, m_treeFrq, 1.0f);
                            float ht = terrain.terrainData.GetInterpolatedHeight(normX, normZ);

                            if (noise > 0.0f && ht < m_terrainHeight * 0.4f)
                            {

                                GameObject obj = Instantiate(cliff[Random.Range(0, 4)],
                                     new Vector3(worldPosX - 1024 + offsetX / unit, ht, worldPosZ - 1024 + offsetZ / unit),
                                     Quaternion.Euler(new Vector3(-90f, 0f, 0f))) as GameObject;

                            }
                    //    }
                    //}
                }
            }
        }

    }
    void FillSpice(Terrain terrain, int tileX, int tileZ)
    {
        Random.seed = 55;


        for (int x = 0; x < m_terrainSize; x += spiceSpacing)
        {
            for (int z = 0; z < m_terrainSize; z += spiceSpacing)
            {
                float unit = 1.0f / (m_terrainSize - 1);

                float offsetX = Random.value * unit * spiceSpacing;
                float offsetZ = Random.value * unit * spiceSpacing;

                float normX = x * unit + offsetX;
                float normZ = z * unit + offsetZ;

                // Get the steepness value at the normalized coordinate.
                float angle = terrain.terrainData.GetSteepness(normX, normZ);

                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                float frac = angle / 90.0f;

                if (frac < 0.5f) //make sure tree are not on steep slopes
                {
                    float worldPosX = x + tileX * (m_terrainSize - 1);
                    float worldPosZ = z + tileZ * (m_terrainSize - 1);

                    float noise = m_treeNoise.FractalNoise2D(worldPosX, worldPosZ, 3, m_treeFrq, 1.0f);
                    float ht = terrain.terrainData.GetInterpolatedHeight(normX, normZ);

                    if (noise > 0.0f && ht < m_terrainHeight * 0.4f)
                    {

                        GameObject obj = Instantiate(spice,
                             new Vector3(worldPosX - 1024 + offsetX / unit, ht, worldPosZ - 1024 + offsetZ / unit),
                             Quaternion.Euler(new Vector3(-90f, 0f, 0f))) as GameObject;

                    }
                }
            }
        }

    }
    void FillWater(Terrain terrain, int tileX, int tileZ)
    {
        Random.seed = 23;


        for (int x = 0; x < m_terrainSize; x += waterSpacing)
        {
            for (int z = 0; z < m_terrainSize; z += waterSpacing)
            {
                float unit = 1.0f / (m_terrainSize - 1);

                float offsetX = Random.value * unit * waterSpacing;
                float offsetZ = Random.value * unit * waterSpacing;

                float normX = x * unit + offsetX;
                float normZ = z * unit + offsetZ;

                // Get the steepness value at the normalized coordinate.
                float angle = terrain.terrainData.GetSteepness(normX, normZ);

                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                float frac = angle / 90.0f;

                if (frac < 0.5f) //make sure tree are not on steep slopes
                {
                    float worldPosX = x + tileX * (m_terrainSize - 1);
                    float worldPosZ = z + tileZ * (m_terrainSize - 1);

                    float noise = m_treeNoise.FractalNoise2D(worldPosX, worldPosZ, 3, m_treeFrq, 1.0f);
                    float ht = terrain.terrainData.GetInterpolatedHeight(normX, normZ);

                    if (noise > 0.0f && ht < m_terrainHeight * 0.4f)
                    {

                        GameObject obj = Instantiate(water[Random.Range(0,3)],
                             new Vector3(worldPosX - 1024 + offsetX / unit, ht, worldPosZ - 1024 + offsetZ / unit),
                             Quaternion.Euler(new Vector3(-90f, 0f, 0f))) as GameObject;

                    }
                }
            }
        }

    }
    void FillCliffs2(Terrain terrain, int tileX, int tileZ)
    {
        Random.seed = 5;


        for (int z = 0; z < m_terrainSize; z +=  m_terrainSize / 5)
        {
            float fakeZ = z;
            for (int x = 0; x < m_terrainSize; x += 1)
            {
               
                       // takenPositions.Add(new Vector2(x, z));
                fakeZ +=0.3f;
                float unit = 1.0f / (m_terrainSize - 1);

                float offsetX = Random.value * unit * cliffSpacing;
                float offsetZ = Random.value * unit * cliffSpacing;

                float normX = x * unit + offsetX;
                float normZ = fakeZ * unit + offsetZ;


               
                    float worldPosX = x + tileX * (m_terrainSize - 1);
                    float worldPosZ = fakeZ + tileZ * (m_terrainSize - 1);

                    float noise = m_treeNoise.FractalNoise2D(worldPosX, worldPosZ, 3, m_treeFrq, 1.0f);
                    float ht = terrain.terrainData.GetInterpolatedHeight(normX, normZ);

                    if (noise > 0.0f && ht < m_terrainHeight * 0.4f)
                    {
                        float q = unit * 10;
                        if( worldPosZ - 1024 + offsetZ /q<1024&&worldPosZ - 1024 + offsetZ / q>-1024)
                        { 
                        GameObject obj = Instantiate(cliff[Random.Range(3, 6)],
                             new Vector3(worldPosX - 1024 + offsetX / unit, ht, worldPosZ - 1024 + offsetZ / q),
                             Quaternion.Euler(new Vector3(-90f, Random.Range(0,180), 0f))) as GameObject;
                        }
                    
                }
            }
        }

    }
	void FillDetailMap(Terrain terrain, int tileX, int tileZ)
	{
		//each layer is drawn separately so if you have a lot of layers your draw calls will increase 
		int[,] detailMap0 = new int[m_detailMapSize,m_detailMapSize];
		int[,] detailMap1 = new int[m_detailMapSize,m_detailMapSize];
		int[,] detailMap2 = new int[m_detailMapSize,m_detailMapSize];
		
		float ratio = (float)m_terrainSize/(float)m_detailMapSize;
		
		Random.seed = 0;
	
		for(int x = 0; x < m_detailMapSize; x++) 
		{
            for (int z = 0; z < m_detailMapSize; z++) 
			{
				detailMap0[z,x] = 0;
				detailMap1[z,x] = 0;
				detailMap2[z,x] = 0;
					
				float unit = 1.0f / (m_detailMapSize - 1);

                float normX = x * unit;
                float normZ = z * unit;
                
                // Get the steepness value at the normalized coordinate.
                float angle = terrain.terrainData.GetSteepness(normX, normZ);
                
                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                float frac = angle / 90.0f;
				
				if(frac < 0.5f)
				{
					float worldPosX = (x+tileX*(m_detailMapSize-1))*ratio;
					float worldPosZ = (z+tileZ*(m_detailMapSize-1))*ratio;
					
					float noise = m_detailNoise.FractalNoise2D(worldPosX, worldPosZ, 3, m_detailFrq, 1.0f);
					
					if(noise > 0.0f) 
					{
						float rnd = Random.value;
						//Randomly select what layer to use
						if(rnd < 0.33f)
							detailMap0[z,x] = 1;
						else if(rnd < 0.66f)
							detailMap1[z,x] = 1;
						else
							detailMap2[z,x] = 1;
					}
				}
				
			}
		}
		
		terrain.terrainData.wavingGrassStrength = m_wavingGrassStrength;
		terrain.terrainData.wavingGrassAmount = m_wavingGrassAmount;
		terrain.terrainData.wavingGrassSpeed = m_wavingGrassSpeed;
		terrain.terrainData.wavingGrassTint = m_wavingGrassTint;
		terrain.detailObjectDensity = m_detailObjectDensity;
		terrain.detailObjectDistance = m_detailObjectDistance;
		terrain.terrainData.SetDetailResolution(m_detailMapSize, m_detailResolutionPerPatch);
		
		terrain.terrainData.SetDetailLayer(0,0,0,detailMap0);
		terrain.terrainData.SetDetailLayer(0,0,1,detailMap1);
		terrain.terrainData.SetDetailLayer(0,0,2,detailMap2);
		
	}
	

}


