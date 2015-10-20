using UnityEngine;
using System.Collections;

public class WormNavigation : MonoBehaviour {

    public GameObject body;
    public GameObject sign;

    public GameObject Waypoint;

    private NavMeshAgent nav;

    private GameObject player;

    private string state;

    private Vector3 targetPosition;

    private Vector3 playerPosition;
    private Vector2 randomPosition;


    // Use this for initialization
    void Start () {
        randomPosition = (Random.insideUnitCircle * GetComponent<SphereCollider>().radius);

        targetPosition = new Vector3(randomPosition.x, 0, randomPosition.y);

        Waypoint.transform.position = targetPosition;

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

                if (Vector3.Distance(nav.transform.position, Waypoint.transform.position) < nav.radius + 1f)
                {
                    randomPosition = (Random.insideUnitCircle * GetComponent<SphereCollider>().radius);
                    targetPosition = new Vector3(randomPosition.x + transform.position.x, 0, randomPosition.y + transform.position.z);

                    Waypoint.transform.position = targetPosition;

                    Debug.Log("New target.");
                }

                nav.SetDestination(Waypoint.transform.position);

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
