using UnityEngine;
using System.Collections;

public class Vision : MonoBehaviour {

	public bool isVisionActive = false;
	private SphereCollider VisionTrigger;
	// Use this for initialization
	void Start () {
		VisionTrigger = GetComponent<SphereCollider>();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			ActivateVision();
		}
		if(Input.GetKeyUp(KeyCode.E))
		{
			DeactivateVision();
		}
	}
	
	void ActivateVision()
	{
		isVisionActive = true;
		VisionTrigger.enabled = true;
		//Debug.Log("Vision " +  isVisionActive);
		
	}
	
	void DeactivateVision()
	{
		isVisionActive = false;
		VisionTrigger.enabled = false;
		//Debug.Log("Vision " +  isVisionActive);
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Vision") && isVisionActive )
		{
			Debug.Log("Vision" + col.gameObject.name);
			col.GetComponent<AffectedByVision>().enabled = true;
			/*
			if(col.tag == "Spice")
			{
				//col.GetComponent<AffectedByVision>()
			}
			else if(col.tag == "Water")
			{

			}
			*/
		}
	}

	
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Vision") && isVisionActive )
		{
			Debug.Log("Exit" + col.gameObject.name);
			col.GetComponent<AffectedByVision>().enabled = false;
		}
	}

}
