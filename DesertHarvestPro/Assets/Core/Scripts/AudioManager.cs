using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource music;
	public AudioSource pickSpice;
	public AudioSource pickWater;


	// Use this for initialization
	void Start () {
	
	}

	public void musicPlay()
	{
		music.Play();
	}

	public void musicStop()
	{
		music.Stop();
	}

	public void pickSpicePlay()
	{
		pickSpice.Play();
	}

	public void pickSpiceStop()
	{
		pickSpice.Stop();
	}
	
	public void pickWaterPlay()
	{
		pickWater.Play();
	}

	
	public void pickWaterStop()
	{
		pickWater.Stop();
	}

}
