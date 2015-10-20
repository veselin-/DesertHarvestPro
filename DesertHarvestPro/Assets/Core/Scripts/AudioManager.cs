using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource MenuMusic;
	public AudioSource GameMusic;
	public AudioSource pickSpice;
	public AudioSource pickWater;
	public AudioSource click;
	public AudioSource vision;


	// Use this for initialization
	void Start () {
	
	}

	public void MenuMusicPlay()
	{
		MenuMusic.Play();
	}

	public void MenuMusicStop()
	{
		MenuMusic.Stop();
	}

	public void GameMusicPlay()
	{
		GameMusic.Play();
	}
	
	public void GameMusicStop()
	{
		GameMusic.Stop();
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

	public void clickPlay()
	{
		click.Play();
	}
	
	
	public void clickStop()
	{
		click.Stop();
	}

	public void visionPlay()
	{
		vision.Play();
	}
	
	
	public void visionStop()
	{
		vision.Stop();
	}

}
