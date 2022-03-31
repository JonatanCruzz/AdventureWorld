using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
public class ConsumableItem : Item
{

    public int healthRestored;
    public List<ScriptableBuff> buffs;
    // public int manaRestored;
    // public int staminaRestored;

    public override void AddBuff(Player player)
    {
        player.hp.HP += healthRestored;
        player.hp.HP = Mathf.Clamp(player.hp.HP, 0, player.hp.Max_HP);
        foreach (var buff in buffs)
        {
            Debug.Log("Adding buff: " + buff.name + " of type: " + buff.GetType().Name);
            player.buffs.AddBuff(buff.InitializeBuff(player.gameObject));
        }
        Debug.Log("buffs size:" + buffs.Count);
        // player.mana += manaRestored;
        // player.stamina += staminaRestored;
    }

}
