using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour {

	public List<GameObject>enemyList;
	private GameObject player;
	public float enemyVisibleDistance = 100f;
	//public int maxEnemies = 5;

	public float CheckInterval = 5f;
	//private List<GameObject> currentActiveEnemies;
	//private int enemyCounter = 0;
	// Use this for initialization
	void Start () {
	
		player = GameObject.FindObjectOfType<PlayerManager>().gameObject;
		enemyList = new List<GameObject>();
		//currentActiveEnemies = new List<GameObject>();

		

		CheckDistance();
		
		InvokeRepeating("CheckDistance", Time.timeSinceLevelLoad, CheckInterval);
	}

	public void CheckDistance()
	{
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject e in enemies)
        {
            enemyList.Add(e);
        }

		foreach(GameObject enemy in enemyList)
		{
			float distance = Vector3.Distance(enemy.transform.position,player.transform.position);

			//if not within the distance then make it inactive
			if(distance > enemyVisibleDistance)
			{
				enemy.SetActive(false);
				/*
				if(currentActiveEnemies.Contains(enemy))
				{
					currentActiveEnemies.Remove(enemy);
					enemyCounter--;
				}
				*/
			}
			else //if within distance check closest
			{
				//enemyCounter++;
				enemy.SetActive(true);
				/*
				if(enemyCounter > maxEnemies && !currentActiveEnemies.Contains(enemy))
				{
					enemy.SetActive(false);
					enemyCounter--;
				}
				else
				{
					currentActiveEnemies.Add(enemy);
					//enemy.SetActive(true);
					enemyCounter++;
				}
				*/

				/*
				if(enemyCounter < maxEnemies && !enemy.activeSelf)
				{

					//enemyCounter++;
					enemy.SetActive(true);
					Debug.Log(enemyCounter);
				}
				*/
			}
		}

	}

}
