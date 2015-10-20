using UnityEngine;
using System.Collections;

public class WormNavigation : MonoBehaviour {

    private NavMeshAgent nav;

    private GameObject player;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        nav.SetDestination(player.transform.position);
    }
	
	// Update is called once per frame
	void Update () {

        
        nav.SetDestination(player.transform.position);

	}
}
