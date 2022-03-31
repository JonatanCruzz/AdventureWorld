using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class NPCController : Interactable
{
    [SerializeField] Dialog dialog;

    public override void OnClick(Player player)
    {

        // dialog
        // var DialogUi = Resources.FindObjectsOfTypeAll<DialogManager>()[0];
        // DialogUi.gameObject.SetActive(true);

        // StartCoroutine(DialogUi.ShowDialog(dialog));
        DialogueManager.instance.StartConversation("NPC 1");


    }
    public override bool isClicked()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

}
