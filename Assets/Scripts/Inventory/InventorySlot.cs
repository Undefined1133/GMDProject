using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	public Image icon;
	public Button removeButton;
	Item item;

public void AddItem(Item newItem)
{
	item = newItem;
	icon.sprite = item.icon;
	icon.enabled = true;
	
	removeButton.interactable = true;
	
}
public void ClearSlot()
{
	item = null;
	icon.sprite = null;
	icon.enabled = false;
	removeButton.interactable = false;
}

public void OnRemoveButton()
{
	Debug.Log("Remove button clicked");
	Inventory.instance.Remove(item);
}

public void UseItem()
{
	Debug.Log("Use button clicked");

	if(item != null)
	{
		item.Use();
	}
}
}
