using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class NPCController : Interactable
{
    [SerializeField] Dialog dialog;

    protected override void OnClick(Player player)
    {

        // dialog
        // var DialogUi = Resources.FindObjectsOfTypeAll<DialogManager>()[0];
        // DialogUi.gameObject.SetActive(true);

        // StartCoroutine(DialogUi.ShowDialog(dialog));
        DialogueManager.instance.StartConversation("NPC 1");


    }
   

}
