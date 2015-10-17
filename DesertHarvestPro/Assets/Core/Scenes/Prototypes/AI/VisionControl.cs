using UnityEngine;
using System.Collections;

public class VisionControl : MonoBehaviour {

    public float FoV = 110f;

    public bool PlayerIsVisible = false;

    public float sightRadius = 10f;

    public Vector3 lastKnownPlayerPosition;

	// Use this for initialization
	void Start () {

        GetComponent<SphereCollider>().radius = sightRadius;

	}
	
	// Update is called once per frame
	void Update () {

      

    }

    void OnTriggerStay(Collider col)
    {

        PlayerIsVisible = false;

        if (col.tag == "Player")
        {

            Vector3 direction = col.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);


           // Debug.DrawRay(transform.position + transform.up, direction.normalized, Color.green, 2f);
            if (angle < FoV * 0.5f)
            {

                Debug.Log("PLayer is in cone");
                RaycastHit hit;


                if (Physics.Raycast(transform.position, direction.normalized, out hit, 100f))
                {


                if (hit.collider.gameObject.tag == "Player")
                    {
                        PlayerIsVisible = true;
                        Debug.Log("Player in vicinity at " + angle + " degrees at a distance of " + hit.distance);
                        lastKnownPlayerPosition = hit.collider.transform.position;
                    }
                }
            }
        }


    }

    void OnTriggerExit(Collider col)
    {
        // If the player leaves the trigger zone...
        if (col.tag == "Player")
            // ... the player is not in sight.
            PlayerIsVisible = false;
    }
}
