using UnityEngine;
using System.Collections;

public class RotateTornado : MonoBehaviour {

    public float rotate = 1.0f;
    private ParticleSystem ps;
	// Use this for initialization
	void Start () {
        ps = gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, rotate * Time.deltaTime, 0);
	}

    IEnumerator transformTornado(){
      //  ps.constantForce.relativeForce = new Vector3();
        return null;
    }
}
