using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI stackAmount;
    private Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        //Since it is required to make a new prefab in order to remove that button from equipment slots
        //I decided to instead use an if statement here, and remove the reference to the button in equipment slots.
        //Overall bad decision, but i dont have much time left so i will leave it like that for now
        if (removeButton != null)
        {
            removeButton.interactable = true;
        }
        if (item is Potion potion)
        {
            if (potion.stackSize != 0) stackAmount.text = potion.stackSize.ToString();
        }
    }

    public Item getItem()
    {
        if (item != null)
        {
            return item;
        }
        return null;
    }

    public void SetStackAmount(string stack)
    {
        stackAmount.text = stack;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        if (removeButton != null)
        {
            removeButton.interactable = false;
        }
    }

    public void OnRemoveButton()
    {
        stackAmount.text = "";
        Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        if (!string.IsNullOrEmpty(stackAmount.text))
        {
            var stackAmountInt = int.Parse(stackAmount.text);
            if (stackAmountInt > 1)
            {
                stackAmountInt--;
                stackAmount.text = stackAmountInt.ToString();
            }
            else if (stackAmountInt == 1)
            {
                stackAmount.text = "";
            }
        }

        if (item != null) item.Use();
    }
}