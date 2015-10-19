using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public float CollectableAmount = 10f;
	private AudioManager aManager;
	// Use this for initialization
	void Start () {
		aManager = GameObject.FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag.Equals("Player"))
		{
			//CollectableAmount = CollectableAmount / 100f;
			if(transform.parent.tag.Equals("Spice"))
			{
				aManager.pickSpicePlay();
				col.GetComponent<PlayerManager>().AddSpice(CollectableAmount);
			}
			else if(transform.parent.tag.Equals("Water"))
			{
				aManager.pickWaterPlay();
				col.GetComponent<PlayerManager>().AddWater(CollectableAmount);
			}

			gameObject.SetActive(false);
			transform.parent.gameObject.SetActive(false);
		}
	}

}
