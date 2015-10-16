using UnityEngine;
using System.Collections;

public class AffectedByVision : MonoBehaviour {

	private string tag;

	public Material spiceVisionMat;
	public Material waterVisionMat;
	private Material defaultMat;
	private Shader bufferShader;

	// Use this for initialization
	void Start () {
		tag = gameObject.tag;

		if(GetComponent<MeshRenderer>())
		{
			defaultMat = GetComponent<MeshRenderer>().material;
		}
		else if(GetComponent<SkinnedMeshRenderer>())
		{
			defaultMat = GetComponent<SkinnedMeshRenderer>().material;
		}

		bufferShader = Resources.Load<Shader>("Custom/ItemGlow");
	}


	void OnEnable()
	{
		if(tag == "Spice")
		{
			ApplySpiceVision();
		}
		else if(tag == "Water")
		{
			ApplyWaterVision();
		}
	}

	void OnDisable()
	{
		GetComponent<MeshRenderer>().material = defaultMat;
	}

	void ApplySpiceVision()
	{
		Texture Base = GetComponent<MeshRenderer>().material.GetTexture("_MainTex");
		Texture NormalMap = GetComponent<MeshRenderer>().material.GetTexture("_BumpMap");
//		Material spiceMat = new Material();
		//spiceMat.shader
		GetComponent<MeshRenderer>().material = spiceVisionMat;
	}

	void ApplyWaterVision()
	{
		GetComponent<MeshRenderer>().material = waterVisionMat;
	}

	Material CreateMaterial(Color emissionColor)
	{
//		Material m = new Material(outlineBufferShader);
//		m.SetColor("_ColorTint", emissionColor);

		/*
		m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		m.SetInt("_ZWrite", 0);
		m.DisableKeyword("_ALPHATEST_ON");
		m.EnableKeyword("_ALPHABLEND_ON");
		m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		m.renderQueue = 3000;
		*/
//		return m;
		return null;
	}

}
