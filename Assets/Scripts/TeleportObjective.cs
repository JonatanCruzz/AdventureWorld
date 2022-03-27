using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

public class TeleportObjective : MonoBehaviour
{
    // description of the objective
    public string objectiveDescription;
    public GameObject targetMap;

    public IEnumerator OnClick()
    {
        Debug.Log("Teleporting to " + objectiveDescription);
        Debug.Log("targetMap: " + targetMap);

        // do something
        var player = GameObject.FindGameObjectWithTag("Player");
        var fade = GameObject.Find("Fade");
        var fadeScript = fade.GetComponent<Fade>();
        player.GetComponent<Animator>().enabled = false;
        player.GetComponent<Player>().enabled = false;

        yield return fadeScript.FadeIn();


        player.transform.position = transform.GetChild(0).transform.position;
        Camera.main.GetComponent<CameraMovements>().setBound(targetMap);
        yield return fadeScript.FadeOut();

        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<Player>().enabled = true;
    }
}
