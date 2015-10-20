using UnityEngine;
using System.Collections;

public class WormNavigation : MonoBehaviour {

    public GameObject body;
    public GameObject sign;

    private NavMeshAgent nav;

    private GameObject player;

    private string state;

    private Vector3 targetPosition;

    private Vector3 playerPosition;


    // Use this for initialization
    void Start () {
        targetPosition = Vector3.zero;

        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponentInChildren<NavMeshAgent>();
        state = "Wander";
        //nav.SetDestination(player.transform.position);
    }
	
	// Update is called once per frame
	void Update () {

        switch (state)
        {
            case "Wander":

                if(Vector3.Distance(nav.transform.position, targetPosition) < 2)
                targetPosition = Random.insideUnitCircle * GetComponent<SphereCollider>().radius;


                nav.SetDestination(targetPosition);

                break;
            case "Hunt":

                nav.SetDestination(playerPosition);

                if (Vector3.Distance(nav.transform.position, playerPosition) < 2)
                    state = "Attack";

                break;
            case "Attack":



                break;



        }


        
       // nav.SetDestination(player.transform.position);

	}

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            playerPosition = col.transform.position;
            state = "Hunt";
        }
        else
        {
            state = "Wander";
        }
    }
    }
