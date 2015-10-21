using UnityEngine;
using System.Collections;

public class WormAttack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if(!col.gameObject.GetComponent<PlayerManager>().playerDead)
            { 
                col.gameObject.GetComponent<PlayerManager>().Die();
            }
        }
    }
}
