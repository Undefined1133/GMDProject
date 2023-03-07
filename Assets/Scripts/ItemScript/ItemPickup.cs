using UnityEngine;

public class ItemPickup : Interactable
{
	public Item item;
	public override void Interact()
	{
		base.Interact();
		
		PickUp();
	}
	
	void PickUp()
	{
		Debug.Log("Picking up " + item.name);
		bool wasPickedUp = Inventory.instance.Add(item);
		//If added to inventory, destroy the object
		if(wasPickedUp)
		{
			Destroy(gameObject);
		}
		
	}
}
