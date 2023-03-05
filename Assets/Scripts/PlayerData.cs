using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
   public int level;
   public float gold;
   public float[] position;
   
   public PlayerData(PlayerManager playerManager)
   {
   	 //Set data
	 gold = playerManager.gold;
	 position = new float[3];
	 GameObject player = playerManager.player;
	 
	 position[0] = player.transform.position.x;
	 position[1] = player.transform.position.y;
	 position[2] = player.transform.position.z;

   }
}
