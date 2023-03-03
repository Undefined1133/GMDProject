using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
	public GameObject goldDrop;
	public float minGoldToDrop;
	public float maxGoldToDrop;
	private int goldPileAmount = 0; 
   public override void Die()
	{
		base.Die();
		Gold gold = goldDrop.GetComponent<Gold>();
		gold.SetAmount(minGoldToDrop, maxGoldToDrop);
		DropCoinsOnGround(goldDrop, transform.position, 3f);
		//Add death animation
		
		Destroy(gameObject);
	}
	public void DropCoinsOnGround(GameObject goldDrop,  Vector3 position, float radius)
	{
	//Amount of GoldPiles to be a random number from 1 - 10 
	goldPileAmount = Random.Range(1,10);
	// Get a reference to the active terrain in the scene
	Terrain terrain = Terrain.activeTerrain;

	for(int i = 0; i < goldPileAmount; i++)
	{
	Vector3 randomPoint = position + Random.insideUnitSphere * radius;
	// Use Terrain.SampleHeight to get the height of the terrain at the random point
	float terrainHeight = terrain.SampleHeight(randomPoint);
	// Instantiate the object at the random point with the y-coordinate set to the terrain height
	randomPoint.y = terrainHeight;
	Instantiate(goldDrop, randomPoint, Quaternion.identity);
	}

	}
}
