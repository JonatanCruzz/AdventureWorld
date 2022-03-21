using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayStatue : Interactable
{
    public string objectiveDescription;
    public SuperTiled2Unity.SuperMap targetMap;


    public void Awake()
    {
        // hide the first child of the statue
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public override void OnClick()
    {
        // if the player is not in the teleport dialog
        var TeleportUI = Resources.FindObjectsOfTypeAll<TeleportManager>()[0];
        TeleportUI.gameObject.SetActive(true);

        //find gameobject with component TeleportManager
    }

    public override bool isClicked()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public async void doTeleport()
    {
        Debug.Log("Teleporting to " + objectiveDescription);
        Debug.Log("targetMap: " + targetMap);

        // do something
        var player = GameObject.FindGameObjectWithTag("Player");
        var fade = GameObject.Find("Fade");
        var fadeScript = fade.GetComponent<Fade>();
        player.GetComponent<Animator>().enabled = false;
        player.GetComponent<Player>().enabled = false;


        await fadeScript.FadeInAsync();


        player.transform.position = transform.GetChild(0).transform.position;
        Debug.Log("targetMap: " + targetMap);

        Camera.main.GetComponent<CameraMovements>().setBound(targetMap.gameObject);

        await fadeScript.FadeOutAsync();

        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<Player>().enabled = true;
    }
}
