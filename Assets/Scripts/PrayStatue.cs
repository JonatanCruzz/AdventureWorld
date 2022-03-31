using System.Collections;
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

    public override void OnClick(Player p)
    {
        // if the player is not in the teleport dialog
        var TeleportUI = Resources.FindObjectsOfTypeAll<TeleportManager>()[0];
        TeleportUI.Display = true;


    }

    public override bool isClicked()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public IEnumerator doTeleport()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var fadeScript = GameObject.Find("Fade").GetComponent<Fade>();
        player.GetComponent<Animator>().enabled = false;
        player.GetComponent<Player>().movePrevent = true;
        yield return StartCoroutine(fadeScript.FadeIn());


        player.transform.position = transform.GetChild(0).transform.position;


        Camera.main.GetComponent<CameraMovements>().setBound(targetMap.gameObject);
        yield return StartCoroutine(fadeScript.FadeOut());

        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<Player>().movePrevent = false;
    }
}
