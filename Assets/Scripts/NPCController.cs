using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : Interactable
{
    [SerializeField] Dialog dialog;

    public override void OnClick()
    {
        // dialog
        var DialogUi = Resources.FindObjectsOfTypeAll<DialogManager>()[0];
        DialogUi.gameObject.SetActive(true);

        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));

    }
    public override bool isClicked()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

}
