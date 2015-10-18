using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Image WaterFill;
	public Image SpiceFill;

	private float animSpeed = 0.2f;

	// Use this for initialization
	void Start () {
		
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
