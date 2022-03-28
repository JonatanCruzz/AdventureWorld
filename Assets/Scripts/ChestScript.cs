using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : Interactable
{
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
        FindObjectOfType<InventoryUI>().OpenChest(chestInventory);

    }

    public override bool isClicked()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
}
