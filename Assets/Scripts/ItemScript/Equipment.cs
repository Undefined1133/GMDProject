using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipmentSlot;
    public int armorModifier;
    public int damageModifier;
    public int minDamageModifier;
    public int maxDamageModifier;
    public bool isEquipped;

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