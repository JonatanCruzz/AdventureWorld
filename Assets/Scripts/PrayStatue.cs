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
        Debug.Log("fading");
        await fadeScript.FadeInAsync();


        player.transform.position = transform.GetChild(0).transform.position;
        Debug.Log("targetMap: " + targetMap);
        Debug.Log("setbound1");

        Camera.main.GetComponent<CameraMovements>().setBound(targetMap.gameObject);
        Debug.Log("setbound2");
        await fadeScript.FadeOutAsync();
        Debug.Log("faded");

        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<Player>().enabled = true;
    }
}
