using UnityEngine;

public class CharacterStats : MonoBehaviour
{
public Stat damage;
public Stat armor;
public Stat maxHealth;
public Stat currentHealth;
public Stat movementSpeed;
public Stat maxMana;
public Stat currentMana;

void Awake()
{
	currentHealth.SetValue(maxHealth.GetValue());
	currentMana.SetValue(maxMana.GetValue());
}
void Update()
{

}

public virtual void TakeDamage(int damage)
{
damage -= armor.GetValue();
damage = Mathf.Clamp(damage, 0, int.MaxValue);
	
currentHealth.SetValue(currentHealth.GetValue() - damage);
Debug.Log(transform.name + " takes " + damage + " damage.");
	
if(currentHealth.GetValue() <= 0)
{
	Die();
}
}

public virtual void Die()
{
	//Die
	//THIS METHOD HAS TO BE OVERWRITTEN
	Debug.Log(transform.name + " died.");
}
}
