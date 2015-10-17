﻿using UnityEngine;
using System.Collections;

public class AffectedByVision : MonoBehaviour {

	
	private float minOutline = 0.005f;
	private float maxOutline = 0.025f;
	//public float lerpSpeed = 1f;
	private bool reverse = true; 
	private float cycleSpeed = 0.025f;

	public Material spiceVisionMat;
	public Material waterVisionMat;

	private Material defaultMat;
	private string selfTag;

	void Awake()
	{
		selfTag = gameObject.tag;

		if(GetComponent<MeshRenderer>())
		{
			defaultMat = GetComponent<MeshRenderer>().material;
		}
		else if(GetComponent<SkinnedMeshRenderer>())
		{
			defaultMat = GetComponent<SkinnedMeshRenderer>().material;
		}
	}

	// Use this for initialization
	void Start () {


	}
	
	void OnEnable()
	{

		if(selfTag.Equals("Spice"))
		{
			ApplySpiceVision();
		}
		else if(selfTag.Equals("Water"))
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

		//spiceVisionMat = CreateMaterial(Color.red);
//		Material spiceMat = Tag Material();
		//spiceMat.shader
		//Material _tempMap = new Material(spiceVisionMat);
		//GetComponent<MeshRenderer>().material = _tempMap;
		Material mat = new Material(spiceVisionMat);
		//mat.SetFloat("_Outline", 0f);
		GetComponent<MeshRenderer>().material = mat;
	}

	void ApplyWaterVision()
	{
		Material _tempMap = new Material(waterVisionMat);
		GetComponent<MeshRenderer>().material = _tempMap;
	}

	void ApplyStepsVision()
	{

	}

	/*
	IEnumerator SpicePingPongOutline()
	{
		Material mat = new Material(spiceVisionMat);
		mat.SetFloat("_Outline", 0f);
		GetComponent<MeshRenderer>().material = mat;

		while(true)
		{
			if(!reverse)
			{
				mat.SetFloat("_Outline", mat.GetFloat("_Outline") + cycleSpeed * Time.deltaTime);
				if(mat.GetFloat("_Outline") >= maxOutline - 0.001f)
				{
					reverse = true;
				}
			}
			else
			{
				mat.SetFloat("_Outline", mat.GetFloat("_Outline") - cycleSpeed * Time.deltaTime);
				if(mat.GetFloat("_Outline") <= minOutline + 0.001f)
				{
					reverse = false;
				}
			}
			yield return null;
		}

		yield return null;

	}

*/


}
