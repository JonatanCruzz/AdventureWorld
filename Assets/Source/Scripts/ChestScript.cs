using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : Interactable
{
    public Inventory defaultInventory;
    private Inventory chestInventory;

    protected override void OnClick(Player p)
    {
        Debug.Log("Opening chest");
        if (chestInventory == null)
        {
            // chestInventory = Instantiate(defaultInventory);
            // chestInventory.transform.position = transform.position;
        }
        // FindObjectOfType<InventoryUI>().OpenChest(chestInventory);

    }
}
