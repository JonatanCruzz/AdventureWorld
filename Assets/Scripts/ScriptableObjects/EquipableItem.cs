
using UnityEngine;

[CreateAssetMenu(fileName = "Equipable", menuName = "Items/Equipable")]
public class EquipableItem : Item
{
    public EquipableItemSlotType itemSlot;
    public int armor;
    public int strength;
    public int agility;
    public int intellect;
    public int stamina;
    public int staminaRegen;
    public int health;
    public int healthRegen;
    public int mana;
    public int manaRegen;
    public int critChance;
    public int critDamage;



    public void Awake()
    {
        itemType = ItemType.Equipable;
    }


    public void onEquip()
    {
        Debug.Log("Equipped " + itemName);
    }

    public override void AddBuff(Player player)
    {
        // player.equipableItems.Add(this);
    }

}