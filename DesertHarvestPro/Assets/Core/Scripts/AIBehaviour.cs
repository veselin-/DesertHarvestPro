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

    // Use this for initialization
    void Start () {

        nav = GetComponent<AICharacterControl>();

        waypointIndex = 0;

        currentWaypoint = Waypoints[waypointIndex];

        state = "Patrol";

        nav.target = currentWaypoint.transform;

    }
	
	// Update is called once per frame
	void Update () {

        switch (state)
        {
            case "Idle":

                break;

            case "Combat":

                break;

            case "Patrol":

                nav.agent.speed = 0.5f;
                if (Vector3.Distance(transform.position, currentWaypoint.transform.position) < 2f)
                {
                    
                    waypointIndex++;

                    if (waypointIndex > Waypoints.Length - 1)
                        waypointIndex = 0;

                    currentWaypoint = Waypoints[waypointIndex];
                    nav.target = currentWaypoint.transform;
                }

                
                break;

        }
	}
}
