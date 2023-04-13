using TMPro;
using UnityEngine;

public class Consumable : MonoBehaviour
{
	// Start is called before the first frame update
	public Potion potion;
	public TextMeshProUGUI priceText;
	PlayerManager playerManager;
	Inventory inventory;
	void Start()
	{
		playerManager  = PlayerManager.instance;
		inventory = Inventory.instance;
	}
	
	public void OnBuyButtonPressed()
	{
		float price = int.Parse(priceText.text);
		if(playerManager.gold <= price)
		{ 
			Debug.LogError("Player does not have enough gold to buy " + potion.name);
		}else
		{ 
			Debug.Log("BUYING POTION WITH STACK = " + potion.stackSize);
			playerManager.gold -= price; 
			playerManager.SetGold(playerManager.gold); 
			inventory.Add(potion);
		}
	}

}
