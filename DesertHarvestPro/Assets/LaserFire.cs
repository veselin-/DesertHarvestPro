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

    public void ShootCall(GameObject p)
    {
        StartCoroutine(Shoot(p));
    }


    IEnumerator Shoot(GameObject p)
    {
        linerenderer.SetPosition(0, barrel.transform.position);

        linerenderer.SetPosition(1, p.transform.position);

        yield return new WaitForSeconds(0.3f);

    }
}
