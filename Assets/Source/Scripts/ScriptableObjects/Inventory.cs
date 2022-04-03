using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Normal", order = 1)]
public class Inventory : ScriptableObject
{
    public List<InventorySlot> items = new List<InventorySlot>();
    public int numberOfKey;
    public int maxSlots = 10;

    public int AddItem(Item item, int amount = 1)
    {
        int itemsLeft = amount;
        if (item.stackable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].AddItem(item, itemsLeft, out itemsLeft))
                {
                    break;
                }
            }
            if (itemsLeft > 0)
            {
                // a while to to create a new slot until the item is added 
                // or the inventory is full
                while (itemsLeft > 0)
                {
                    if (items.Count >= maxSlots)
                    {
                        Debug.Log("Inventory is full");
                        break;
                    }
                    else
                    {
                        var slot = new InventorySlot(item, 0);
                        slot.AddItem(item, itemsLeft, out itemsLeft);
                        items.Add(slot);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < maxSlots && itemsLeft > 0; i++)
            {
                if (items[i] == null)
                {
                    items[i] = new InventorySlot(item, 1);
                    itemsLeft--;
                }
                else if (items[i].item == null)
                {
                    items[i].AddItem(item, itemsLeft, out itemsLeft);
                }
            }
            if (itemsLeft > 0)
            {
                Debug.Log("Inventory is full");
            }
        }
        return itemsLeft;
    }




}