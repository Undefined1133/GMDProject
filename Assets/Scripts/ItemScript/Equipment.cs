using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipmentSlot;
    public int armorModifier;
    public int damageModifier;
    public int minDamageModifier;
    public int healthModifier;
    public int speedModifier;
    public int maxDamageModifier;
    public bool isEquipped;

    // Add this method to return a copy of the equipment
    public Equipment GetCopy()
    {
        Equipment copy = CreateInstance<Equipment>();
        copy.name = name;
        copy.equipmentSlot = equipmentSlot;
        copy.isDefaultItem = isDefaultItem;
        copy.icon = icon;
        copy.armorModifier = armorModifier;
        copy.damageModifier = damageModifier;
        copy.minDamageModifier = minDamageModifier;
        copy.healthModifier = healthModifier;
        copy.speedModifier = speedModifier;
        copy.maxDamageModifier = maxDamageModifier;
        copy.isEquipped = false;
        return copy;
    }
    public override void Use()
    {
        base.Use();
        //Equip the item
        //Remove from inventory
        RemoveFromInventory();
        EquipmentManager.instance.Equip(this);
    }
}

public enum EquipmentSlot
{
    Head,
    Chest,
    Legs,
    Weapon,
    Shield,
    Feet
}