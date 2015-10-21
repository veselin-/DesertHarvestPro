using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour {

	public List<GameObject>enemyList;
	private GameObject player;
	public float enemyVisibleDistance = 100f;
	public int maxEnemies = 5;

	private int enemyCounter = 0;
	// Use this for initialization
	void Start () {
	
		player = GameObject.FindObjectOfType<PlayerManager>().gameObject;
		enemyList = new List<GameObject>();

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		foreach(GameObject e in enemies)
		{
			enemyList.Add(e);
		}
		
		InvokeRepeating("CheckDistance", Time.timeSinceLevelLoad, 5f);
	}

	void CheckDistance()
	{
		foreach(GameObject enemy in enemyList)
		{
			float distance = Vector3.Distance(enemy.transform.position,player.transform.position);
			if(distance > enemyVisibleDistance)
			{
				enemy.SetActive(false);
			}
			else
			{
				if(enemyCounter < maxEnemies && !enemy.activeSelf)
				{
					enemyCounter++;
					enemy.SetActive(true);
				}
			}
		}
	}

	/*
	public PoolSystem(int size, GameObject prefab){
		enemyList = new List<GameObject>();
		for(int i = 0 ; i < size; i++){
			GameObject obj = (GameObject)Instantiate(prefab);
			enemyList.Add(obj);
		}
	}


	public GameObject GetObject(){
		if(enemyList.Count > 0){ 
			GameObject obj = enemyList[0];
			obj.RemoveAt(0);
			return obj;
		}
		return null;
	}

	public void DestroyObjectPool(GameObject obj){
		enemyList.Add(obj);
		obj.SetActive(false);
	}

	public void ClearPool(){
		for(int i = enemyList.Count - 1; i > 0; i--){
			GameObject obj = enemyList.RemoveAt(i);
			Destroy(obj);
		}
		enemyList = null;
	}

*/
}
