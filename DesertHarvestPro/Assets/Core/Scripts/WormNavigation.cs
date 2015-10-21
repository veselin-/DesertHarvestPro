using UnityEngine;
using System.Collections;

public class WormNavigation : MonoBehaviour {

    public GameObject body;
    public GameObject sign;
    public GameObject signRest;

    private Vector3 signStartPos;
    private bool showSign;
    private float signLerp;

    public GameObject Waypoint;

    private NavMeshAgent nav;

    private GameObject player;

    private string state;

    private Vector3 targetPosition;

    private Vector3 playerPosition;
    private Vector2 randomPosition;

    private NavMeshPath path;

   private bool hasAttacked;


    // Use this for initialization
    void Start () {
        hasAttacked = false;

        randomPosition = (Random.insideUnitCircle * GetComponent<SphereCollider>().radius);

        targetPosition = new Vector3(randomPosition.x + transform.position.x, transform.position.y + 5, randomPosition.y + transform.position.z);

        Waypoint.transform.position = targetPosition;

        signStartPos = sign.transform.localPosition;

        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponentInChildren<NavMeshAgent>();
        state = "Wander";
        //nav.SetDestination(player.transform.position);
    }
	
	// Update is called once per frame
	void Update () {

        if (showSign)
        {
            signLerp += Time.deltaTime / 8f;
            if (signLerp > 1f)
                signLerp = 1f;
        }
        else
        {
            signLerp -= Time.deltaTime / 8f;
            if (signLerp < 0f)
                signLerp = 0f;
        }

        sign.transform.localPosition = Vector3.Lerp(signStartPos, signRest.transform.localPosition, signLerp);


        switch (state)
        {
            case "Wander":

                showSign = false;
             //   if(!NavMesh.CalculatePath(nav.transform.position, Waypoint.transform.position, NavMesh.AllAreas, path))
              //  {
              //      randomPosition = (Random.insideUnitCircle * GetComponent<SphereCollider>().radius);
             //       targetPosition = new Vector3(randomPosition.x + transform.position.x, transform.position.y, randomPosition.y + transform.position.z);
             //
             //       Waypoint.transform.position = targetPosition;
             //
             //       Debug.Log("New target.");
             //   }

                if (Vector3.Distance(nav.transform.position, Waypoint.transform.position) < nav.radius + 1f)
                {
                    randomPosition = (Random.insideUnitCircle * GetComponent<SphereCollider>().radius);
                    targetPosition = new Vector3(randomPosition.x + transform.position.x, transform.position.y + 5, randomPosition.y + transform.position.z);

                    Waypoint.transform.position = targetPosition;

                    Debug.Log("New target.");
                }

                nav.SetDestination(Waypoint.transform.position);

                //Hide wormsign



                break;

            case "Hunt":

                Debug.Log("isHunting");
                showSign = true;

                nav.SetDestination(playerPosition);

                if (Vector3.Distance(nav.transform.position, playerPosition) < 2)
                    state = "Attack";



                break;

            case "Attack":

                nav.Stop();

                if (!hasAttacked)
                {
                    body.GetComponent<Animation>().Play("Emerge");
                    hasAttacked = true;
                }
                if (!body.GetComponent<Animation>().isPlaying)
                {
                    state = "Wander";
                    hasAttacked = false;
                }


                break;



        }


        
       // nav.SetDestination(player.transform.position);

	}

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            playerPosition = col.transform.position;

            if(state != "Attack")
            state = "Hunt";
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            state = "Wander";
        }
    }

    }
