using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class AIBehaviour : MonoBehaviour {

    private AICharacterControl nav;

    private string state;
    private string subState;

    public GameObject[] Waypoints;
    private GameObject currentWaypoint;
    private int waypointIndex;

    private VisionControl vision;
    private float searchTimer;

    private bool isLooking;
    // Use this for initialization
    void Start () {

        vision = GetComponent<VisionControl>();
        nav = GetComponent<AICharacterControl>();

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
                Debug.Log("Searching...");

                nav.agent.speed = 0.8f;

                searchTimer += Time.deltaTime;

                if (vision.PlayerIsVisible)
                {
                    state = "Combat";
                    searchTimer = 0f;
                    StopCoroutine("searchTurn");
                    break;
                }
                else
                {
                    nav.target = vision.lastKnownPlayerPosition;
                    if (!isLooking)
                    {
                        isLooking = true;
                        StartCoroutine(searchTurn());
                    }
                }



                if(!vision.PlayerIsVisible && searchTimer > 20f)
                {
                    state = "Patrol";
                    searchTimer = 0f;
                    StopCoroutine("searchTurn");
                }

                break;

            //Combat: See player = Attack. If player is out of range = Follow player. PLayer out of sight = Search.
            case "Combat":

                isLooking = false;

                Debug.Log("Combating...");

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

                Debug.Log("Patrolling...");

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

        Quaternion startRot = transform.rotation;
        Quaternion endRot = new Quaternion(transform.rotation.x, transform.rotation.y - 180, transform.rotation.z, 0);

        transform.rotation = Quaternion.Slerp(startRot,endRot, 1 * Time.deltaTime);

        yield return null;
    }
}
