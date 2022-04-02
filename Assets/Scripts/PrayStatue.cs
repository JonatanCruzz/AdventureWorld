using System.Collections;
using PixelCrushers;
using UnityEngine;

public class PrayStatue : Interactable
{
    public string objectiveDescription;
    public BaseCameraMargin targetMap;


    public void Awake()
    {
        // hide the first child of the statue
        transform.GetChild(0).gameObject.SetActive(false);
    }

    protected override void OnClick(Player p)
    {
        // if the player is not in the teleport dialog
        SaveSystem.SaveToSlot(0);
        var TeleportUI = Resources.FindObjectsOfTypeAll<TeleportManager>()[0];
        TeleportUI.Display = true;


    }

    public IEnumerator doTeleport()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var fadeScript = GameObject.Find("Fade").GetComponent<Fade>();
        player.GetComponent<Player>().movePrevent = true;
        player.GetComponent<Player>().canInteract = false;
        yield return StartCoroutine(fadeScript.FadeIn());


        player.transform.position = transform.GetChild(0).transform.position;


        Camera.main.GetComponent<CameraMovements>().setBound(targetMap.gameObject);
        yield return StartCoroutine(fadeScript.FadeOut());

        player.GetComponent<Player>().movePrevent = false;
        player.GetComponent<Player>().canInteract = true;
    }
}
