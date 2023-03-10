using System;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
	#region Singleton
	
 public static EquipmentManager instance;
 void Awake()
 {
 	instance = this;
 }
 
 #endregion

public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
public OnEquipmentChanged onEquipmentChanged;
Equipment[] currentEquipment;
Inventory inventory;
void Start()
{
	inventory = Inventory.instance;
	int numSlots = Enum.GetNames(typeof(EquipmentSlot)).Length;
	currentEquipment = new Equipment[numSlots];
}

public void Equip(Equipment newItem)
{
	int slotIndex = (int)newItem.equipmentSlot;
	Equipment oldItem = null;
	if(currentEquipment[slotIndex] != null)
	{
		oldItem = currentEquipment[slotIndex];
		inventory.Add(oldItem);
	}
	
	if(onEquipmentChanged != null)
	{
		onEquipmentChanged.Invoke(newItem, oldItem);
	}
	
	currentEquipment[slotIndex] = newItem;
}
public void Unequip(int slotIndex)
{
	if(currentEquipment[slotIndex] != null)
	{
		Equipment oldItem = currentEquipment[slotIndex];
		
		if (inventory.Add(oldItem))
		{
		 currentEquipment[slotIndex] = null;
		}
		
		if(onEquipmentChanged != null)
	 	{
		    onEquipmentChanged.Invoke(null, oldItem);
		}
	}
}

public void UnequipAll()
{
	for(int i = 0; i< currentEquipment.Length; i++)
	{
		Unequip(i);
	}
}
void Update()
{
	if(Input.GetKeyDown(KeyCode.U))
	{
		UnequipAll();
	}
}

}
