using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
   public int level;
   public float gold;
   public float[] position;
   public int health;
   
   
   public PlayerData(PlayerManager playerManager)
   {
   	 //Set data
	 PlayerStats playerStats = playerManager.player.GetComponent<PlayerStats>();
	 GameObject player = playerManager.player;

	 gold = playerManager.gold;
	 health = playerStats.currentHealth;
	 position = new float[3];
	 
	 position[0] = player.transform.position.x;
	 position[1] = player.transform.position.y;
	 position[2] = player.transform.position.z;

   }
}
