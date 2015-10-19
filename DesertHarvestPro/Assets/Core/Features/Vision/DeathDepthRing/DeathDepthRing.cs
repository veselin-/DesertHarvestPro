﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DeathDepthRing : MonoBehaviour {
	
	public Material mat;
	public LayerMask everything;
	public LayerMask nothing;
	
	void Start () {
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}

	/*
	void Update (){

		if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1)){
			//set _StartingTime to current time
			mat.SetFloat("_StartingTime", Time.time);
			//set _RunRingPass to 1 to start the ring
			mat.SetFloat("_RunRingPass", 1);
		}
		else if (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(1)){
			mat.SetFloat("_StartingTime", Time.time);
			mat.SetFloat("_RunRingPass", 2);
		}

	}
	*/

	void OnEnable()
	{
		transform.GetChild(0).GetComponent<Camera>().cullingMask = nothing;
		GetComponent<Camera>().cullingMask = everything;
		Debug.Log("OnEnable deathring");
		//set _StartingTime to current time
		mat.SetFloat("_StartingTime", Time.time);
		//set _RunRingPass to 1 to start the ring
		mat.SetFloat("_RunRingPass", 1);
	}
	
	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination){
		//mat is the material containing your shader
		Graphics.Blit(source,destination,mat);
	}
}