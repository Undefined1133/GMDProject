using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	public Transform itemsParent;
	public GameObject inventoryUI;
	InventorySlot[] slots;
	Inventory inventory;
	void Start()
	{
		inventory = Inventory.instance;
		inventory.onItemChangedCallBack += UpdateUI;
		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetButtonDown("Inventory"))
		{
			inventoryUI.SetActive(!inventoryUI.activeSelf);
		}
	}
	
	void UpdateUI()
	{
		for(int i =0; i < slots.Length; i++)
		{
			if(i < inventory.items.Count)
			{
				Debug.Log("Updating UI " + inventory.items[i].name);
				slots[i].AddItem(inventory.items[i]);
				SetStackAmount(inventory.items[i], slots[i]);
			}else
			{
				slots[i].ClearSlot();
			}
		}
	}

	void SetStackAmount(Item item, InventorySlot inventorySlot)
	{
		Debug.Log("SET STACK AMOUNT " + item.name);
		if (item is Potion)
		{
			Potion potion = (Potion) item;
			Debug.Log(potion.stackSize + "----> inside set stack amount");
			if (potion.stackSize > 0)
			{
				inventorySlot.SetStackAmount(potion.stackSize.ToString());
			}
		}
		
	}
}
