using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerManager : MonoBehaviour {

	private float SpiceAmount = 10f;
	private float WaterAmount = 50f;

	private float WaterLimit = 80f;
	private float SpiceLimit = 80f;

	private UIManager UI;
	// Use this for initialization
	void Start () {
		UI = GameObject.FindObjectOfType<UIManager>().GetComponent<UIManager>();

		UI.SetSpice(SpiceAmount);
		UI.SetWater(WaterAmount);
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}

	public void UISpice()
	{
		UI.SetSpice(SpiceAmount);
	}

	public void UIWater()
	{
		UI.SetWater(WaterAmount);
	}

	public void AddSpice(float amount)
	{
		if(SpiceAmount < SpiceLimit)
		{
			SpiceAmount += amount;
			if(SpiceAmount >= SpiceLimit)
			{
				SpiceAmount = SpiceLimit;
				WinGame();
			}
		}
		UISpice();
	}
	
	public void RemoveSpice(float amount)
	{
		SpiceAmount -= amount;
		if(SpiceAmount < 0)
		{
			SpiceAmount = 0;
		}
		UISpice();
	}

	public void AddWater(float amount)
	{
		if(WaterAmount < WaterLimit)
		{
			WaterAmount += amount;
			if(WaterAmount > WaterLimit)
			{
				WaterAmount = WaterLimit;
			}
		}
		UIWater();
	}

	public void RemoveWater(float amount)
	{
		WaterAmount -= amount;
		if(WaterAmount <= 0)
		{
			WaterAmount = 0;
			Die();
		}
		UIWater();
	}

	public void Die()
	{
		//Time.timeScale = 0;
		GetComponent<ThirdPersonUserControl>().enabled = false;
		UI.DeathScreen.SetActive(true);
		Camera.main.GetComponent<DeathDepthRing>().enableBlast = true;
	}

	public void WinGame()
	{
		GetComponent<ThirdPersonUserControl>().enabled = false;
		UI.WinScreen.SetActive(true);
	}

	/*
	public void LoseGame()
	{

	}
	*/
}
