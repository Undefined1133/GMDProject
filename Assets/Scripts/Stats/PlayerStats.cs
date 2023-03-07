using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
	// Start is called before the first frame update
	public HealthBar healthBar;
	public ManaBar manaBar;

	void Start()
	{
		healthBar.SetMaxHealth(maxHealth.GetValue());
		manaBar.SetMaxMana(maxMana.GetValue());
		EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
	}
	
	void Update()
	{
	if(Input.GetKeyDown(KeyCode.T))
	{
		TakeDamage(damage.GetValue());
	}
	}
	
	void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
	{
		if(newItem != null)
		{
		armor.AddModifier(newItem.armorModifier);
		damage.AddModifier(newItem.damageModifier);
		}
		if(oldItem != null)
		{
			armor.RemoveModifier(oldItem.armorModifier);
			damage.RemoveModifier(oldItem.damageModifier);
		}
	
	}

	public override void Die()
	{
		base.Die();
		//Kill the player somehow

		PlayerManager.instance.KillPlayer();
	}
	public override void TakeDamage(int damage)
	{
	damage -= armor.GetValue();
	damage = Mathf.Clamp(damage, 0, int.MaxValue);
	
	currentHealth.SetValue(currentHealth.GetValue() - damage);
	Debug.Log(transform.name + " takes " + damage + " damage.");
	
	if(currentHealth.GetValue() <= 0)
	{
		Die();
	}
	healthBar.SetHealth(currentHealth.GetValue());
	}
}
