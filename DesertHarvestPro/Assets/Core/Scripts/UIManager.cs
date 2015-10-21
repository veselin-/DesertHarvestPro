using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Image WaterFill;
	public Image SpiceFill;
	public GameObject DeathScreen;
	public GameObject WinScreen;
	public GameObject PauseScreen;

	private bool pause = false;
	private float animSpeed = 0.2f;
	private AudioManager aManager;
	// Use this for initialization
	void Start () {
		aManager = GameObject.FindObjectOfType<AudioManager>();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(PauseScreen != null)
			{
				aManager.clickPlay();
				PauseGame();
			}
		}
	}

	public void SetSpice(float amount)
	{
		amount = amount / 100f;
		float finalAmount = SpiceFill.fillAmount + amount;
		StartCoroutine(FillSpiceAnimation(amount));
		//SpiceFill.fillAmount = amount;
	}

	public void SetWater(float amount)
	{
		amount = amount / 100f;
		float finalAmount = WaterFill.fillAmount + amount;
		StartCoroutine(FillWaterAnimation(amount));
		//WaterFill.fillAmount = amount;
	}

	public void RestartLevel()
	{
		aManager.clickPlay();
		Application.LoadLevel("Terrain");
	}

	public void MainMenu()
	{
		aManager.clickPlay();
		Application.LoadLevel("MainMenu");
	}

	public void QuitGame()
	{
		aManager.clickPlay();
		Application.Quit();
	}

	public void StartGame()
	{
		aManager.clickPlay();
		Application.LoadLevel("Terrain");
	}

	public void PauseGame()
	{
		pause = !pause;
		if(pause)
		{
            Cursor.visible = true;
			Time.timeScale = 0;
			PauseScreen.SetActive(true);
		}
		else
		{
            Cursor.visible = false;
			PauseScreen.SetActive(false);
			Time.timeScale = 1;
		}
	}

	IEnumerator FillSpiceAnimation(float finalAmount)
	{
		if(SpiceFill.fillAmount > finalAmount)
		{
			while(SpiceFill.fillAmount > finalAmount)
			{
				SpiceFill.fillAmount -= animSpeed * Time.deltaTime;
				yield return null;
			}
		}
		else
		{
			while(SpiceFill.fillAmount < finalAmount)
			{
				SpiceFill.fillAmount += animSpeed * Time.deltaTime;
				yield return null;
			}
		}

		SpiceFill.fillAmount = finalAmount;
		yield return null;
	}



	IEnumerator FillWaterAnimation(float finalAmount)
	{
		if(WaterFill.fillAmount > finalAmount)
		{
			while(WaterFill.fillAmount > finalAmount)
			{
				WaterFill.fillAmount -= animSpeed * Time.deltaTime;
				yield return null;
			}
		}
		else
		{
			while(WaterFill.fillAmount < finalAmount)
			{
				WaterFill.fillAmount += animSpeed * Time.deltaTime;
				yield return null;
			}
		}
		
		WaterFill.fillAmount = finalAmount;
		yield return null;
	}




}
