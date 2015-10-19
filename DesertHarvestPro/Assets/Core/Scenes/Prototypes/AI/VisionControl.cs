using UnityEngine;
using System.Collections;

public class VisionControl : MonoBehaviour {

    public float FoV = 110f;

    public bool PlayerIsVisible = false;

    public float sightRadius = 10f;

    public Vector3 lastKnownPlayerPosition;

    public GameObject eyes;

	// Use this for initialization
	void Start () {

        GetComponent<SphereCollider>().radius = sightRadius;
	}
	
	// Update is called once per frame
	void Update () {
      //  Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
      //  Debug.DrawRay(transform.position + transform.up * 1.4f, forward, Color.green);
        //Debug.DrawRay(transform.position + transform.up * 1.4f, direction.normalized, Color.green, 0f);
        //Debug.DrawRay(transform.position + transform.up * 1.4f, transform.position + transform.forward * 20, Color.green, 0f);
    }

    void OnTriggerStay(Collider col)
    {

        PlayerIsVisible = false;

        if (col.tag == "Player")
        {
            
            Vector3 direction = col.transform.position - eyes.transform.position;

            
            float angle = Vector3.Angle(direction, eyes.transform.forward);
           

            // Debug.DrawLine(transform.position, transform.position + transform.forward*20, Color.green, 2f);
            if (angle < FoV * 0.5f)
            {

                Debug.Log(direction.normalized);
                RaycastHit hit;
              //  Debug.DrawRay(eyes.transform.position, direction, Color.green, 0f);

                if (Physics.Raycast(eyes.transform.position, direction, out hit, 100f))
                {
                    
                    Debug.Log(hit.collider.name);
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
