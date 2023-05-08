using TMPro;
using UnityEngine;

public class Consumable : MonoBehaviour
{
	// Start is called before the first frame update
	public Potion potion;
	public TextMeshProUGUI priceText;
	PlayerManager playerManager;
	Inventory inventory;
	private LogManager logManager;
	void Start()
	{
		logManager = LogManager.instance;
		playerManager  = PlayerManager.instance;
		inventory = Inventory.instance;
	}
	
	public void OnBuyButtonPressed()
	{
		float price = int.Parse(priceText.text);
		if(playerManager.gold <= price)
		{ 
			logManager.ErrorLog("You don't have enough gold!");
		}else
		{ 
			playerManager.RemoveGold(price);
			inventory.Add(potion.GetCopy());
		}
	}

}
