using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion" )]
public class Potion : Item
{
	PlayerManager playerManager;
	PlayerStats playerStats;
	PlayerController playerController;
	public int healthModifier;
	public int manaModifier;
	public int speedModifier;
	public int expModifier;
	public int stackSize = 1;


	void Start()
	{
		playerManager = PlayerManager.instance;
		if (playerManager != null)
		{
			playerStats = playerManager.player.GetComponent<PlayerStats>();
			playerController = playerManager.player.GetComponent<PlayerController>();
			Debug.Log(playerStats);
		}else
		{
			Debug.LogError("Player Manager is null for some reason :D");
		}
	}
	

	public override void Use()
	{
		Start();
		base.Use();
		if (playerStats != null)
		{
			AddHealth();
			AddMana();
			AddMoveSpeed();
			AddExpRate();
		}

		Debug.Log("Stack size on USE = " + stackSize);
		if (stackSize == 1)
		{
			RemoveFromInventory();
		}
		else
		{
			stackSize--;
		}
	}
	
	void AddHealth()
	{
		if (healthModifier != 0 && playerStats != null)
		{
			playerStats.Heal(healthModifier);
		}
	}
	void AddMoveSpeed()
	{
		if (speedModifier != 0)
		{
			Debug.Log("Current speed " + playerController.speed + " Modifier " + speedModifier + " playerstats movement speed " + playerStats.movementSpeed.GetValue());
			playerStats.movementSpeed.AddModifier(speedModifier);
			if(playerController.speed < playerStats.movementSpeed.GetValue())
			{
				playerStats.movementSpeed.SetValue(playerStats.movementSpeed.GetValue());
				playerController.SetSpeed(playerStats.movementSpeed.GetValue());
				playerStats.OnSpeedChanged();
			}else
			{ 
				Debug.Log("Your speed is too big for that potion, try a bigger one :D"); 
			}
			playerStats.movementSpeed.RemoveModifier(speedModifier);
		}
	}
	void AddExpRate()
	{
		if (expModifier != 0)
		{
				Debug.Log("SHOULD BE ADDING EXP GAIN % :D");
		}
	}
	void AddMana()
	{
		if (manaModifier != 0)
		{
			playerStats.currentMana.AddModifier(manaModifier);
			if(playerStats.currentMana.GetValue() < playerStats.maxMana.GetValue())
			{
				playerStats.currentMana.SetValue(playerStats.currentMana.GetValue());
				playerStats.currentMana.RemoveModifier(manaModifier);
				playerManager.manaBar.SetMana(playerStats.currentMana.GetValue());
				Debug.Log(playerStats.currentMana.GetValue());
			}else
			{
				playerStats.currentMana.SetValue(playerStats.maxMana.GetValue());
				playerStats.currentMana.RemoveModifier(manaModifier);
				playerManager.manaBar.SetMana(playerStats.currentMana.GetValue());
				Debug.Log(playerStats.currentMana.GetValue());
			}
		}
	}
}
