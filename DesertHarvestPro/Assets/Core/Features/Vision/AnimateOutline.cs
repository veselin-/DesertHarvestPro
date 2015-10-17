using UnityEngine;
using System.Collections;

public class AnimateOutline : MonoBehaviour {

	private Material mat;

	public float minOutline = 0.01f;
	public float maxOutline = 0.03f;
	//public float lerpSpeed = 1f;
	private bool reverse = true; 
	public float cycleSpeed = 0.01f;

	// Use this for initialization
	void Start () {


		if(transform.GetComponent<SkinnedMeshRenderer>())
		{
			mat = transform.GetComponent<SkinnedMeshRenderer>().material;
		}
		else
		{
			mat = transform.GetComponent<MeshRenderer>().material;
		}

		//StartCoroutine(IncreaseOutline());
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!reverse)
		{
			//mat.SetFloat("_Outline", Mathf.Lerp(mat.GetFloat("_Outline"), maxOutline, lerpSpeed * Time.deltaTime));
			mat.SetFloat("_Outline", mat.GetFloat("_Outline") + cycleSpeed * Time.deltaTime);
			//Debug.Log(mat.GetFloat("_Outline"));
			if(mat.GetFloat("_Outline") >= maxOutline - 0.001f)
			{
				reverse = true;
				//Debug.Log(reverse);
			}
		}
		else
		{
			mat.SetFloat("_Outline", mat.GetFloat("_Outline") - cycleSpeed * Time.deltaTime);
			//mat.SetFloat("_Outline", Mathf.Lerp(mat.GetFloat("_Outline"), minOutline, lerpSpeed * Time.deltaTime));
			if(mat.GetFloat("_Outline") <= minOutline + 0.001f)
			{
				reverse = false;
				//Debug.Log(reverse);
			}
		}
	}

	/*
	IEnumerator IncreaseOutline()
	{

		while(mat.GetFloat("_Outline") <= maxOutline)
		{
			mat.SetFloat("_Outline", Mathf.Lerp(minOutline, maxOutline, lerpSpeed * Time.deltaTime));
		}

		StartCoroutine(DecreaseOutline());
		yield return null;
	}

	IEnumerator DecreaseOutline()
	{
		
		while(mat.GetFloat("_Outline") >= minOutline)
		{
			mat.SetFloat("_Outline", Mathf.Lerp(minOutline, maxOutline, lerpSpeed * Time.deltaTime));
		}
		
		StartCoroutine(IncreaseOutline());
		yield return null;
	}
	*/

}
