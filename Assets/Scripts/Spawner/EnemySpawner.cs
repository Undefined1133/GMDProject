using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	[SerializeField]
	private GameObject enemyPrefab; 
	[SerializeField]
	private float spawnInterval;
	void Start()
	{
		StartCoroutine(spawnEnemy(enemyPrefab, spawnInterval));
	}
	
	private IEnumerator spawnEnemy(GameObject enemy, float spawnInterval)
	{
		yield return new WaitForSeconds(spawnInterval);
		if(enemy != null)
		{
		Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-3f, 3), Random.Range(-3f, 3), 0);
		GameObject enemyToSpawn = Instantiate(enemy, spawnPosition, Quaternion.identity);
		StartCoroutine(spawnEnemy(enemy, spawnInterval));
		}
	}
}
