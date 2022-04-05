using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipable", menuName = "AdventureWorld/Items/Equipable")]
public class EquipableItem : Item
{
    [Title("Equipment Type")] [EnumToggleButtons]
    public EquipableItemSlotType itemSlot;

    [FoldoutGroup("Stats")] public int armor;
    [FoldoutGroup("Stats")] public int strength;

    [FoldoutGroup("Stats")] public int agility;

    // public int intellect;
    // public int stamina;
    // public int staminaRegen;
    [FoldoutGroup("Stats")] public int health;

    [FoldoutGroup("Stats")] public int healthRegen;
    // public int mana;
    // public int manaRegen;
    // public int critChance;
    // public int critDamage;

    public void onEquip()
    {
        Debug.Log("Equipped " + ItemName);
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