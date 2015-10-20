using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Vision : MonoBehaviour {

	public bool isVisionActive = false;
	private SphereCollider VisionTrigger;
	public List<GameObject> VisibleObjects;

	private float minRim = 1f;
	private float maxRim = 3f;
	//public float lerpSpeed = 1f;
	private bool reverse = true; 
	private float cycleSpeed = 6f;

	public LayerMask activeVisionMain;
	public LayerMask activeVisionPlayer;
	public LayerMask PlayerCameraMask;
	public LayerMask MainCamMask;
	private PlayerManager pm;

	public float consumeSpice = 5f;
	public float consumeSpiceOverTime = 1f;

	private Camera playerCam;
	// Use this for initialization
	void Start () {
		VisionTrigger = GetComponent<SphereCollider>();
		VisibleObjects = new List<GameObject>();
		playerCam = Camera.main.transform.GetChild(0).GetComponent<Camera>();
		pm = transform.parent.GetComponent<PlayerManager>();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
		{
			ActivateVision();
		}
		if(Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(1))
		{
			DeactivateVision();
		}


		if(isVisionActive)
		{
			foreach(GameObject g in VisibleObjects)
			{
				Material mat = g.GetComponent<MeshRenderer>().material;

				/*
				if(mat.GetFloat("_RimPower") >= maxRim)
				{
					float rimPower = mat.GetFloat("_RimPower") - cycleSpeed * Time.deltaTime;
					mat.SetFloat("_RimPower", Mathf.Clamp(rimPower, minRim, maxRim));

				}
				else if(mat.GetFloat("_RimPower") <= maxRim)
				{
					float rimPower = mat.GetFloat("_RimPower") + cycleSpeed * Time.deltaTime;
					mat.SetFloat("_RimPower", Mathf.Clamp(rimPower, minRim, maxRim));
				}
				*/


				if(!reverse)
				{
					//float debug = cycleSpeed * Time.deltaTime;
					if(mat.HasProperty("_RimPower"))
					{
						float rimPower = mat.GetFloat("_RimPower") + cycleSpeed * Time.deltaTime;
						//Debug.Log("if " + debug);
						mat.SetFloat("_RimPower", Mathf.Clamp(rimPower, minRim, maxRim));
						if(mat.GetFloat("_RimPower") >= maxRim)
						{
							reverse = true;
						}
					}
				}
				else
				{
					//float debug = cycleSpeed * Time.deltaTime;
					//Debug.Log("else " + debug);
					if(mat.HasProperty("_RimPower"))
					{
						float rimPower = mat.GetFloat("_RimPower") - cycleSpeed * Time.deltaTime;
						mat.SetFloat("_RimPower", Mathf.Clamp(rimPower, minRim, maxRim));
						if(mat.GetFloat("_RimPower") <= minRim)
						{
							reverse = false;
						}
					}
				}


			}

		}

	}

	void OnDisable()
	{

	}

	void ActivateVision()
	{
		if(pm.GetSpice() > 0)
		{
			Material mat = Camera.main.GetComponent<DepthRingPass>().mat;
			//set _StartingTime to current time
			mat.SetFloat("_StartingTime", Time.timeSinceLevelLoad);
			//set _RunRingPass to 1 to start the ring
			mat.SetFloat("_RunRingPass", 1);

			isVisionActive = true;
			VisionTrigger.enabled = true;
			Camera.main.cullingMask = activeVisionMain;
			playerCam.cullingMask = activeVisionPlayer;
			StartCoroutine(lerpCameraFar());
			//Debug.Log("Vision " +  isVisionActive);
			//pm.RemoveSpice(consumeSpice);
			StartCoroutine(ConsumeSpiceWhileVision());
			//InvokeRepeating("ConsumeSpiceWhileVision", Time.time, consumeSpiceOverTime);
		}
		else
		{
			Debug.Log("Not Enough spice");
		}
		
	}
	
	void DeactivateVision()
	{
		Material mat = Camera.main.GetComponent<DepthRingPass>().mat;
		//set _StartingTime to current time
		mat.SetFloat("_StartingTime", Time.timeSinceLevelLoad);
		//set _RunRingPass to 1 to start the ring
		mat.SetFloat("_RunRingPass", 0);

		ClearVisibleObjects();
		isVisionActive = false;
		VisionTrigger.enabled = false;
		Camera.main.cullingMask = MainCamMask;
		playerCam.cullingMask = PlayerCameraMask;
		StopCoroutine(lerpCameraFar());
		playerCam.farClipPlane = 5f;
		StopCoroutine(ConsumeSpiceWhileVision());
		//Debug.Log("Vision " +  isVisionActive);
	}

	IEnumerator ConsumeSpiceWhileVision()
	{
		while(pm.GetSpice() > 0 && isVisionActive)
		{
			pm.RemoveSpice(consumeSpice);
			yield return new WaitForSeconds(consumeSpiceOverTime);
		}

		DeactivateVision();
		yield return null;
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
	

	IEnumerator lerpCameraFar()
	{
		while(playerCam.farClipPlane < 100)
		{
			playerCam.farClipPlane += Time.deltaTime * 40;
			yield return null;
		}

		yield return null;
	}

}
