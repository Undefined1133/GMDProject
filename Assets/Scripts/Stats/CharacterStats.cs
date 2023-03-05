using UnityEngine;

public class CharacterStats : MonoBehaviour
{
public Stat damage;
public Stat armor;
public int maxHealth;
public int currentHealth;


void Awake()
{
	currentHealth = maxHealth;
}
void Update()
{

}

	public virtual void TakeDamage(int damage)
    {
	damage -= armor.GetValue();
	damage = Mathf.Clamp(damage, 0, int.MaxValue);
	
	currentHealth -= damage;
	Debug.Log(transform.name + " takes " + damage + " damage.");
	
	if(currentHealth <= 0)
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
