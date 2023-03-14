using System;
using UnityEngine;

[Serializable]
public class PlayerData 
{
   public int level;
   public float gold;
   public float[] position;
   public int health;
   public int mana;
   
   
   public PlayerData(PlayerManager playerManager)
   {
   	 //Set data
	 PlayerStats playerStats = playerManager.player.GetComponent<PlayerStats>();
	 GameObject player = playerManager.player;

	 gold = playerManager.gold;
	 health = playerStats.currentHealth.GetValue();
	 mana = playerStats.currentMana.GetValue();
	 position = new float[3];
	 
	 position[0] = player.transform.position.x;
	 position[1] = player.transform.position.y;
	 position[2] = player.transform.position.z;

   }
}
