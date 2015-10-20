using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class AIBehaviour : MonoBehaviour {

    private AICharacterControl nav;
    private Animator ani;

    private string state;
    private string subState;

    public GameObject[] Waypoints;
    private GameObject currentWaypoint;
    private int waypointIndex;

    private VisionControl vision;
    private float searchTimer;

    private bool isLooking;

    public GameObject gun;
    // Use this for initialization
    void Start () {

        vision = GetComponent<VisionControl>();
        nav = GetComponent<AICharacterControl>();
        ani = GetComponent<Animator>();

        waypointIndex = 0;

        isLooking = false;

        currentWaypoint = Waypoints[waypointIndex];

        state = "Patrol";

        nav.target = currentWaypoint.transform.position;



    }
	
	// Update is called once per frame
	void Update () {

        switch (state)
        {
            //Search: See player = Combat. Unable to find player = Patrol after 5 secs.
            case "Search":
                //    Debug.Log("Searching...");
                ani.SetBool("IsInCombat", false);
                Debug.Log("ExitCombat");
                nav.agent.speed = 0.8f;

                searchTimer += Time.deltaTime;

                if (vision.PlayerIsVisible)
                {
                    state = "Combat";
                    searchTimer = 0f;
                    StopAllCoroutines();
                    break;
                }
                else
                {
                    nav.target = vision.lastKnownPlayerPosition;
                    if (Vector3.Distance( vision.lastKnownPlayerPosition, transform.position) < 2f && !isLooking)
                    {
                        isLooking = true;
                        StartCoroutine(searchTurn());
                    }
                }



                if(!vision.PlayerIsVisible && searchTimer > 20f)
                {
                    state = "Patrol";
                    searchTimer = 0f;
                    StopAllCoroutines();
                }

                break;

            //Combat: See player = Attack. If player is out of range = Follow player. PLayer out of sight = Search.
            case "Combat":

                ani.SetBool("IsInCombat", true);
                Debug.Log("EnterCombat");

                isLooking = false;

              //  Debug.Log("Combating...");

                nav.agent.speed = 1f;

                if(vision.PlayerIsVisible)
                {
                    nav.agent.speed = 0f;
                   Vector3 dir = vision.lastKnownPlayerPosition - transform.position;
                    dir.y = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);
                   // Debug.Log("Bang bang bang!!");
                }
                else
                {
                    state = "Search";
                }

                break;

            //Patrol: Player not detected = Patrol waypoints. See player = Combat.
            case "Patrol":

                isLooking = false;

                ani.SetBool("IsInCombat", false);
                Debug.Log("ExitCombat");

                // Debug.Log("Patrolling...");

                nav.agent.speed = 0.5f;

                nav.target = currentWaypoint.transform.position;
                if (Vector3.Distance(transform.position, currentWaypoint.transform.position) < 2f)
                {
                    
                    waypointIndex++;

                    if (waypointIndex > Waypoints.Length - 1)
                        waypointIndex = 0;

                    currentWaypoint = Waypoints[waypointIndex];
                   
                }

                if (vision.PlayerIsVisible)
                {
                    state = "Combat";
                }

                break;

        }

	}

    IEnumerator searchTurn()
    {
        Debug.Log("Soldier is turning");

        float turnTime = 5f;

        float elapsedTime = 0f;


        while (elapsedTime < turnTime)
        {
            transform.Rotate(0, 90*Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }

        
    }
    void Shoot()
    {

        Debug.Log("Bang!!");
        RaycastHit hit;

        Debug.DrawRay(gun.transform.position, transform.forward*10, Color.red, 0.5f);

        if (Physics.Raycast(gun.transform.position, transform.forward, out hit, 100f))
        {

            // Debug.Log(hit.collider.name);
            if (hit.collider.gameObject.tag == "Player")
            {
                hit.collider.GetComponent<PlayerManager>().Die();
            }
        }
    }
}
