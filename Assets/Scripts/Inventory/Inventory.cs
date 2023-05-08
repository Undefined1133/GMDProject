using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }

    #endregion

    public List<Item> items = new();
    public int space = 20;
    private bool foundExistingItem;

    public delegate void OnItemChanged();

    public OnItemChanged onItemChangedCallBack;

    public bool Add(Item item)
    {
        if (item is Potion potion)
        {
            Debug.Log("Item is potion : " + potion.name);
            // Check if an existing item of the same type is already in the inventory
            foreach (var existingItem in items)
            {
                if (existingItem.name == potion.name)
                {
                    // Cast existingItem to Potion type, and check if it succeeded
                    Potion existingPotion = existingItem as Potion;
                    if (existingPotion != null)
                    {
                        existingPotion.stackSize++;
                        foundExistingItem = true;
                        onItemChangedCallBack?.Invoke();
                        Debug.Log("Stack size = " + existingPotion.stackSize);
                    }
                    //Breaking if an item has been found :D
                    break;
                }
            }
        }
        else if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.LogError("Not enough space in Inventory.");
                return false;
            }

            items.Add(item);
            onItemChangedCallBack?.Invoke();
        }

        if (!foundExistingItem && item is Potion)
        {
            items.Add(item);
            Debug.Log("Added new item to inventory: " + item.name);
            onItemChangedCallBack?.Invoke();
        }

        foundExistingItem = false;
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        onItemChangedCallBack?.Invoke();
    }
}