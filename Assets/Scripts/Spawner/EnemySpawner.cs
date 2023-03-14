using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

[SerializeField]
private GameObject enemyPrefab;
[SerializeField]
private float spawnInterval;

private List<GameObject> enemiesInsideTrigger = new List<GameObject>();
private int numEnemies = 0;

void Start()
{
	StartCoroutine(spawnEnemy(enemyPrefab, spawnInterval));
}

private IEnumerator spawnEnemy(GameObject enemy, float spawnInterval)
{
	yield return new WaitForSeconds(spawnInterval);

	if (enemy != null && numEnemies < 3)
	{
		Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-3f, 3), 0, Random.Range(-3f, 3));
		GameObject enemyToSpawn = Instantiate(enemy, spawnPosition, Quaternion.identity);
	}

	StartCoroutine(spawnEnemy(enemy, spawnInterval));
}

void OnTriggerEnter(Collider other)
{
	//Even though for now its obvious it will be enemy, its nice to have
	if (other.CompareTag("Enemy"))
	{
		numEnemies++;
		enemiesInsideTrigger.Add(other.gameObject);
	}
}

void OnTriggerExit(Collider other)
{
	//Even though for now its obvious it will be enemy, its nice to have
	if (other.CompareTag("Enemy"))
	{
		enemiesInsideTrigger.Remove(other.gameObject);
		numEnemies--;
	}
}

void Update()
{
	// Check if any enemies in the trigger have been destroyed
	for (int i = enemiesInsideTrigger.Count - 1; i >= 0; i--)
	{
		if (enemiesInsideTrigger[i] == null)
		{
			enemiesInsideTrigger.RemoveAt(i);
			numEnemies--;
		}
	}
}
}
