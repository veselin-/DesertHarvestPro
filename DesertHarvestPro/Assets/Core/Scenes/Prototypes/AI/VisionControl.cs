using UnityEngine;
using System.Collections;

public class VisionControl : MonoBehaviour {

    public float FoV = 110f;

    public bool PlayerIsVisible = false;

    public float sightRadius = 10f;

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

            if (angle < FoV * 0.5f)
            {




                RaycastHit hit;


                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, sightRadius))
                {

                    if (hit.collider.tag == "Player")
                    {
                        PlayerIsVisible = true;
                        Debug.Log("Player in vicinity at " + angle + "degrees");
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
