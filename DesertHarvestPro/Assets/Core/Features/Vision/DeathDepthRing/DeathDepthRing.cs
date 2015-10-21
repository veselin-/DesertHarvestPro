using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DeathDepthRing : MonoBehaviour {
	
	public Material mat;
	public LayerMask everything;
	public LayerMask nothing;
 
	void Awake()
	{
		mat.SetFloat("_StartingTime", Time.timeSinceLevelLoad);
		mat.SetFloat("_RunRingPass", 0);
	}

	void Start () {
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}


	public void Blast()
	{
		transform.GetChild(0).GetComponent<Camera>().cullingMask = nothing;
		GetComponent<Camera>().cullingMask = everything;
		//set _StartingTime to current time
		mat.SetFloat("_StartingTime", Time.timeSinceLevelLoad);
		//set _RunRingPass to 1 to start the ring
		mat.SetFloat("_RunRingPass", 1);
		StartCoroutine(LerpSineToFront());


	}
	

	IEnumerator LerpSineToFront()
	{
		Material tempMat = GetComponent<SineHeatPostProcess>().mat;
		while(tempMat.GetFloat("_CloseDepth") > 0.1)
		{
			float depth = tempMat.GetFloat("_CloseDepth") - Time.deltaTime;
			tempMat.SetFloat("_CloseDepth", depth);
            //Debug.Log(depth);
			yield return null;
		}
		tempMat.SetFloat("_CloseDepth", 0);
		//mat.SetFloat("_StartingTime", Time.timeSinceLevelLoad);
		//mat.SetFloat("_RunRingPass", 0);
		yield return null;
	}
	
	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination){
		//mat is the material containing your shader
		Graphics.Blit(source,destination,mat);
	}
}