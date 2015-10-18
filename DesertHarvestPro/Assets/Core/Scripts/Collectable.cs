using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public float CollectableAmount = 10f;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag.Equals("Player"))
		{
			//CollectableAmount = CollectableAmount / 100f;
			if(transform.parent.tag.Equals("Spice"))
			{
				col.GetComponent<PlayerManager>().AddSpice(CollectableAmount);
			}
			else if(transform.parent.tag.Equals("Water"))
			{
				col.GetComponent<PlayerManager>().AddWater(CollectableAmount);
			}

			gameObject.SetActive(false);

		}
	}

}
