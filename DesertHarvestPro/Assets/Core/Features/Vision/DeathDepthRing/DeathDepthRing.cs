using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DeathDepthRing : MonoBehaviour {
	
	public Material mat;
	public LayerMask everything;
	public LayerMask nothing;
	public bool enableBlast = false;
	public bool isLerping = false;
	private Material tempMat; 
	void Awake()
	{
		tempMat = new Material(mat);
		tempMat.SetFloat("_RunRingPass", 0);
	}

	void Start () {
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}


	void Update (){


		if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1)){
			//transform.GetChild(0).GetComponent<Camera>().cullingMask = nothing;
			//GetComponent<Camera>().cullingMask = everything;
			//set _StartingTime to current time
			mat.SetFloat("_StartingTime", Time.time);
			//set _RunRingPass to 1 to start the ring
			mat.SetFloat("_RunRingPass", 1);
		}


		/*
		if(enableBlast)
		{
			Blast();
			enableBlast = false;
			isLerping = true;
		}
		if(isLerping)
		{
			Material tempMat2 = GetComponent<SineHeatPostProcess>().mat;
			if(tempMat2.GetFloat("_CloseDepth") > 0.1)
			{
				float depth = tempMat2.GetFloat("_CloseDepth") - Time.deltaTime;
				tempMat2.SetFloat("_CloseDepth", depth);
			}
			else
			{
				isLerping = false;
			}
		}
		*/

	}


	public void Blast()
	{
		transform.GetChild(0).GetComponent<Camera>().cullingMask = nothing;
		GetComponent<Camera>().cullingMask = everything;
		//Debug.Log("OnEnable deathring");
		//set _StartingTime to current time
		tempMat.SetFloat("_StartingTime", Time.time);
		//set _RunRingPass to 1 to start the ring
		tempMat.SetFloat("_RunRingPass", 1);
		//StartCoroutine(LerpSineToFront());


	}

	IEnumerator LerpSineToFront()
	{
		Material tempMat2 = GetComponent<SineHeatPostProcess>().mat;
		while(tempMat2.GetFloat("_CloseDepth") > 0.1)
		{
			float depth = tempMat2.GetFloat("_CloseDepth") - Time.deltaTime;
			tempMat2.SetFloat("_CloseDepth", depth);
			yield return null;
		}
		tempMat2.SetFloat("_CloseDepth", 0);
		yield return null;
	}
	
	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination){
		//mat is the material containing your shader
		Graphics.Blit(source,destination,tempMat);
	}
}