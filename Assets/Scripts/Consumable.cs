using UnityEngine;
using UnityEngine.UI;
using TMPro;


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
		playerManager.gold -= price;
		playerManager.SetGold(playerManager.gold);
		inventory.Add(potion);
		}
		Debug.Log(playerManager.gold);
	}

}
