using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LittleChest : Interactable
{
    public bool isOpen;
    public Inventory defaultInventory;
    private Inventory chestInventory;


    public override void OnClick()
    {
        Debug.Log("Opening chest");
        if (chestInventory == null)
        {
            chestInventory = Instantiate(defaultInventory);
            // chestInventory.transform.position = transform.position;
        }
        isOpen = true;
        var inventoryUI = FindObjectOfType<InventoryUI>();
        inventoryUI.OpenChest(chestInventory);
        inventoryUI.OnInventoryClose += () => { isOpen = false; };


    }

    public override bool IsInteracting()
    {
        return isOpen;
    }

    public override bool isClicked()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
}
