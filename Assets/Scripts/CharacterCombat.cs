using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
	CharacterStats myStats;
	public float attackSpeed = 1f;
	private float attackCooldown = 0f;
	public float attackDelay = .6f;
	
	public event Action OnAttack; 
	void Start() 
	{ 
		myStats = GetComponent<CharacterStats>(); 
	}
	
	void Update() 
	{ 
		attackCooldown -= Time.deltaTime; 
	}
	public void Attack(List<CharacterStats> targetStatsList) 
	{ 
		if(attackCooldown <= 0f && targetStatsList != null) 
		{ 
			foreach (CharacterStats targetStats in targetStatsList) 
			{
				if (targetStats != null && !targetStats.isDead)
				{
					targetStats.TakeDamage(Random.Range(myStats.minDamage.GetValue(), myStats.maxDamage.GetValue()));
				}
			} 
			if(OnAttack != null) 
			{ 
				OnAttack(); 
			} 
			//attackCooldown = 1f / attackSpeed; 
		} 
	}
}
