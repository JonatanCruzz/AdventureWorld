
using UnityEngine;

[CreateAssetMenu(fileName = "Equipable", menuName = "Items/Equipable")]
public class EquipableItem : Item
{
    public EquipableItemSlotType itemSlot;

    public int armor;
    public int strength;
    public int agility;
    // public int intellect;
    // public int stamina;
    // public int staminaRegen;
    public int health;
    public int healthRegen;
    // public int mana;
    // public int manaRegen;
    // public int critChance;
    // public int critDamage;

    public void onEquip()
    {
        Debug.Log("Equipped " + itemName);
    }

    public override void AddBuff(Player player)
    {
        // player.equipableItems.Add(this);
        var hp = player.GetComponent<HealthUnit>();
        hp.AddictiveHP += health;
        Debug.Log("Added " + health + " to HP");
    }

    public override void RemoveBuff(Player player)
    {
        // player.equipableItems.Remove(this);
        var hp = player.GetComponent<HealthUnit>();
        hp.AddictiveHP -= health;
        Debug.Log("Removed " + health + " to HP");

    }

}