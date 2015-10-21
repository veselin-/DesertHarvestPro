using UnityEngine;
using System.Collections;

public class LaserFire : MonoBehaviour {

    private LineRenderer linerenderer;

    public GameObject barrel;

    private GameObject player;

	// Use this for initialization
	void Start () {

        linerenderer = GetComponent<LineRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShootCall(Vector3 p)
    {
        StartCoroutine(Shoot(p));
    }


    IEnumerator Shoot(Vector3 p)
    {

        linerenderer.enabled = true;
        linerenderer.SetPosition(0, barrel.transform.position);

        linerenderer.SetPosition(1, p);

     

        yield return new WaitForSeconds(0.3f);


        linerenderer.enabled = false;
    }
}
