using System;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);

    public OnEquipmentChanged onEquipmentChanged;
    public delegate void EquipEventHandler();

    public event EquipEventHandler equip;
    private Equipment[] currentEquipment;
    private Inventory inventory;
    public InventorySlot head;
    public InventorySlot chest;
    public InventorySlot weapon;
    public InventorySlot shield;
    public InventorySlot legs;
    public InventorySlot feet;


    private void Start()
    {
        inventory = Inventory.instance;
        var numSlots = Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }

    public void Equip(Equipment newItem)
    {
        //Slot index is using enum inside equipment as index
        var slotIndex = (int) newItem.equipmentSlot;
        if (!newItem.isEquipped)
        {
            
            newItem.isEquipped = true;
            UpdateEquippedItemsUI(newItem);
            //Creating a variable to be used if need to swap existing already equipped equipment with new
            Equipment oldItem = null;
            //If already is equipped
            if (currentEquipment[slotIndex] != null)
            {
                //Take the equipped item
                oldItem = currentEquipment[slotIndex];
                oldItem.isEquipped = false;
                //Add it to the inventory
                inventory.Add(oldItem);
            }
            //Invoke change in equipment to update inventory UI
            onEquipmentChanged?.Invoke(newItem, oldItem);
            //Equip equipment
            currentEquipment[slotIndex] = newItem;
            equip?.Invoke();
        }
        else
        {
            Unequip(slotIndex);
            newItem.isEquipped = false;
        }

    }

    public void Unequip(int slotIndex)
    {
        Debug.Log("Unequip triggered :DDD");
        //If something is equipped
        if (currentEquipment[slotIndex] != null)
        {
            //Reference the equipped item
            var oldItem = currentEquipment[slotIndex];
            //Add to inventory equipped item
            if (inventory.Add(oldItem)) currentEquipment[slotIndex] = null;

            onEquipmentChanged?.Invoke(null, oldItem);
        }
        UpdateUnequippedItemsUI(slotIndex);
        equip?.Invoke();
    }

    public void UnequipAll()
    {
        for (var i = 0; i < currentEquipment.Length; i++) Unequip(i);
        equip?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) UnequipAll();
    }

    private void UpdateEquippedItemsUI(Equipment newItem)
    {
        //Slot index is using enum inside equipment as index
        //{Head, Chest, Legs, Weapon, Shield, Feet}
        var slotIndex = (int) newItem.equipmentSlot;
        switch (slotIndex)
        {
            case 0:
                Debug.Log("Thats Head Equipment");
                head.AddItem(newItem);
                break;
            case 1:
                Debug.Log("Thats Chest Equipment");
                chest.AddItem(newItem);
                break;
            case 2:
                Debug.Log("Thats Legs Equipment");
                legs.AddItem(newItem);
                break;
            case 3:
                Debug.Log("Thats Weapon Equipment");
                weapon.AddItem(newItem);
                break;
            case 4:
                Debug.Log("Thats Shield Equipment");
                shield.AddItem(newItem);
                break;
            case 5:
                Debug.Log("Thats Feet Equipment");
                feet.AddItem(newItem);
                break;
        }
    }

    private void UpdateUnequippedItemsUI(int slotIndex)
    {
        //Slot index is using enum inside equipment as index
        //{Head, Chest, Legs, Weapon, Shield, Feet}
        switch (slotIndex)
        {
            case 0:
                Debug.Log("Thats Head Equipment");
                head.ClearSlot();
                break;
            case 1:
                Debug.Log("Thats Chest Equipment");
                chest.ClearSlot();
                break;
            case 2:
                Debug.Log("Thats Legs Equipment");
                legs.ClearSlot();
                break;
            case 3:
                Debug.Log("Thats Weapon Equipment");
                weapon.ClearSlot();
                break;
            case 4:
                Debug.Log("Thats Shield Equipment");
                shield.ClearSlot();
                break;
            case 5:
                Debug.Log("Thats Feet Equipment");
                feet.ClearSlot();
                break;
        }
        
    }
}