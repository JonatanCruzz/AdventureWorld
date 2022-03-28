using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
public class ConsumableItem : Item
{

    public int healthRestored;
    // public int manaRestored;
    // public int staminaRestored;

    public override void AddBuff(Player player)
    {
        player.hp.HP += healthRestored;
        // player.mana += manaRestored;
        // player.stamina += staminaRestored;
    }

}
