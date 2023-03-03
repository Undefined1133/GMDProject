using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactable
{
	PlayerManager playerManager;
	List<CharacterStats> myStats = new List<CharacterStats>();
	
void Start()
{
	playerManager = PlayerManager.instance;
	myStats.Add(GetComponent<CharacterStats>());
}
 public override void Interact()
 {
 	base.Interact(); 
	CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
	if(playerCombat != null)
	{
		playerCombat.Attack(myStats);
	}
 }
}
