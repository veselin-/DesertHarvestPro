using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Vision : MonoBehaviour {

	public bool isVisionActive = false;
	private SphereCollider VisionTrigger;
	public List<GameObject> VisibleObjects;

	// Use this for initialization
	void Start () {
		VisionTrigger = GetComponent<SphereCollider>();
		VisibleObjects = new List<GameObject>();
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


		foreach(GameObject g in VisibleObjects)
		{

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
		ClearVisibleObjects();
		isVisionActive = false;
		VisionTrigger.enabled = false;

		//Debug.Log("Vision " +  isVisionActive);
	}

	public void RemoveFromVisibleObjects(GameObject g)
	{
		if(VisibleObjects.Contains(g))
		{
			g.GetComponent<AffectedByVision>().enabled = false;
			if(g.GetComponent<AnimateOutline>())
			{
				g.GetComponent<AnimateOutline>().enabled = false;
			}
			VisibleObjects.Remove(g);
		}
	}

	void ClearVisibleObjects()
	{
		foreach(GameObject g in VisibleObjects)
		{
			g.GetComponent<AffectedByVision>().enabled = false;
			if(g.GetComponent<AnimateOutline>())
			{
				g.GetComponent<AnimateOutline>().enabled = false;
			}
		}
		VisibleObjects.Clear();
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Vision") && isVisionActive )
		{
			//Debug.Log("Vision" + col.gameObject.name);

			if(!VisibleObjects.Contains(col.gameObject))
			{
				col.GetComponent<AffectedByVision>().enabled = true;
				if(col.GetComponent<AnimateOutline>())
				{
					col.GetComponent<AnimateOutline>().enabled = true;
				}
				VisibleObjects.Add(col.gameObject);
			}

			//col.GetComponent<AffectedByVision>().enabled = true;
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
			if(VisibleObjects.Contains(col.gameObject))
			{
				col.GetComponent<AffectedByVision>().enabled = false;
				if(col.GetComponent<AnimateOutline>())
				{
					col.GetComponent<AnimateOutline>().enabled = false;
				}
				VisibleObjects.Remove(col.gameObject);
			}
		}
	}

}
