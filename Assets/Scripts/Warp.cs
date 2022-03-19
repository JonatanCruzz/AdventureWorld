using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class Warp : MonoBehaviour
{
    public GameObject target;
    public GameObject targetMap;
    public bool needText;

    bool start = false;
    bool isFadeIn = false;
    float alpha = 0;
    float fadeTime = 1f;

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
        other.GetComponent<Animator>().enabled = false;
        other.GetComponent<Player>().enabled = false;

        FadeIn();
        yield return new WaitForSeconds(fadeTime);

        other.transform.position = target.transform.GetChild(0).transform.position;
        Camera.main.GetComponent<CameraMovements>().setBound(targetMap);

        FadeOut();
        other.GetComponent<Animator>().enabled = true;
        other.GetComponent<Player>().enabled = true;

        if (needText)
        {
            StartCoroutine(area.GetComponent<Area>().ShowArea(targetMap.name));
        }
    }

    public void OnGUI()
    {
        if (!start)
            return;

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        Texture2D tex;
        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);

        if (isFadeIn)
        {
            alpha = Mathf.Lerp(alpha, 1.1f, fadeTime * Time.deltaTime);
        }
        else
        {
            alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);
            if (alpha < 0) start = false;

        }
    }

    void FadeIn()
    {
        start = true;
        isFadeIn = true;
    }

    void FadeOut()
    {
        isFadeIn = false;
    }
}
