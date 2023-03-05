using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
	// Start is called before the first frame update
	public HealthBar healthBar;

	void Start()
	{
		healthBar.SetMaxHealth(maxHealth);
		EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
	}
	
	void Update()
	{
	if(Input.GetKeyDown(KeyCode.T))
	{
		TakeDamage(damage.GetValue());
	}
	}

	// Update is called once per frame
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
	
	currentHealth -= damage;
	Debug.Log(transform.name + " takes " + damage + " damage.");
	
	if(currentHealth <= 0)
	{
		Die();
	}
	healthBar.SetHealth(currentHealth);
    }
}
