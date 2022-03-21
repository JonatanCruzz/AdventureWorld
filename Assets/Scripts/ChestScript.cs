using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : Interactable
{
    public override void OnClick()
    {
        Debug.Log("Opening chest");
    }

    public override bool isClicked()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
}
