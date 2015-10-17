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
    // Use this for initialization
    void Start () {

        vision = GetComponent<VisionControl>();
        nav = GetComponent<AICharacterControl>();

        waypointIndex = 0;

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

                nav.agent.speed = 0.8f;

                searchTimer += Time.deltaTime;

                if (vision.PlayerIsVisible)
                {
                    state = "Combat";
                    searchTimer = 0f;
                }
                else
                {
                    nav.target = vision.lastKnownPlayerPosition;
                }

                if(!vision.PlayerIsVisible && searchTimer > 5f)
                {
                    state = "Patrol";
                    searchTimer = 0f;
                }

                break;

            //Combat: See player = Attack. If player is out of range = Follow player. PLayer out of sight = Search.
            case "Combat":

                nav.agent.speed = 1f;

                if (Vector3.Distance(transform.position, vision.lastKnownPlayerPosition) > 10f && vision.PlayerIsVisible)
                {

                    nav.target = vision.lastKnownPlayerPosition;

                }
                else if(vision.PlayerIsVisible)
                {
                    nav.agent.speed = 0f;
                    float step = 1f * Time.deltaTime;
                    Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3( vision.lastKnownPlayerPosition.x, 0f, vision.lastKnownPlayerPosition.y), step, 0.0F);
                    transform.rotation = Quaternion.LookRotation(newDir);
                    Debug.Log("Bang bang bang!!");
                }
                else
                {
                    state = "Search";
                }

                break;

            //Patrol: Player not detected = Patrol waypoints. See player = Combat.
            case "Patrol":

                nav.agent.speed = 0.5f;
                if (Vector3.Distance(transform.position, currentWaypoint.transform.position) < 2f)
                {
                    
                    waypointIndex++;

                    if (waypointIndex > Waypoints.Length - 1)
                        waypointIndex = 0;

                    currentWaypoint = Waypoints[waypointIndex];
                    nav.target = currentWaypoint.transform.position;
                }

                if (vision.PlayerIsVisible)
                {
                    state = "Search";
                }

                break;

        }
	}
}
