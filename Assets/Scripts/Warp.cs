using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;


public class Warp : MonoBehaviour
{
    public GameObject target;
    public GameObject targetMap;
    public bool needText;

    float alpha = 0;

    GameObject area;

    public void Awake()
    {
        Assert.IsNotNull(target);

        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        Assert.IsNotNull(targetMap);

        area = GameObject.FindGameObjectWithTag("Area");
    }

    public IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            yield break;
        }
        // other.GetComponent<Animator>().enabled = false;
        other.GetComponent<Player>().movePrevent = true;
        var fade = GameObject.Find("Fade").GetComponent<Fade>();
        yield return fade.FadeIn();

        other.transform.position = target.transform.GetChild(0).transform.position;
        Camera.main.GetComponent<CameraMovements>().setBound(targetMap);

        // other.GetComponent<Animator>().enabled = true;
        other.GetComponent<Player>().movePrevent = false;
        yield return fade.FadeOut();

        if (needText)
        {
            StartCoroutine(area.GetComponent<Area>().ShowArea(targetMap.name));
        }
    }

}
