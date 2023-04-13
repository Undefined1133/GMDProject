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
   public float exp;
   
   
   public PlayerData(PlayerManager playerManager)
   {
   	 //Set data
	 PlayerStats playerStats = playerManager.player.GetComponent<PlayerStats>();
	 GameObject player = playerManager.player;

	 exp = playerStats.exp;
	 gold = playerManager.gold;
	 health = playerStats.currentHealth.GetValue();
	 mana = playerStats.currentMana.GetValue();
	 position = new float[3];

	 var playersPosition = player.transform.position;
	 position[0] = playersPosition.x;
	 position[1] = playersPosition.y;
	 position[2] = playersPosition.z;

   }
}
