using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	if(attackCooldown <= 0f)
	{
	  foreach (CharacterStats targetStats in targetStatsList)
		{
			StartCoroutine(DoDamage(targetStats, attackDelay));
		}
		if(OnAttack != null)
		{
			OnAttack();
		}
		attackCooldown = 1f / attackSpeed;
	}
}

IEnumerator DoDamage(CharacterStats stats, float delay)
{
	yield return new WaitForSeconds(delay);
	stats.TakeDamage(myStats.damage.GetValue());
}

}
