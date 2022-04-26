using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LittleChest : Interactable
{
    public Inventory defaultInventory;
    private Inventory chestInventory;


    protected override void OnClick(Player player)
    {
        Debug.Log("Opening chest");
        if (chestInventory == null)
        {
            chestInventory = Instantiate(defaultInventory);
            // chestInventory.transform.position = transform.position;
        }
        GameController.Instance.uiManager.OpenChest(chestInventory);


    }


  
}
