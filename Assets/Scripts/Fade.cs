using System;
using System.Threading.Tasks;
using Unity;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float fadeSpeed = 0.8f;
    public bool sceneStarting = true;
    public float alpha = 1.0f;
    public int drawDepth = -1000;
    public float fadeDir = -1;

    public Color DefaultColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    private Color currentColor;

    void OnGUI()
    {
        // fade out/in the alpha value using a direction, speed and Time.deltaTime to convert the operation to seconds
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        // set color of our GUI (in this case our texture). All color values remain the same & the Alpha is set to the alpha variable
        GUI.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
    }

    public float BeginFade(int direction, Color? color = null)
    {
        if (color.HasValue)
        {
            this.currentColor = color.Value;
        }
        else
        {
            this.currentColor = this.DefaultColor;
        }
        fadeDir = direction;
        sceneStarting = true;
        return fadeSpeed;
    }


    void Start()
    {
        FadeInAsync().ContinueWith(t =>
        {
            FadeOutAsync();
        });
    }
    public void FadeIn(Color? color = null)
    {
        FadeInAsync(color);
    }

    public void FadeOut(Color? color = null)
    {
        FadeOutAsync(color);
    }
    public async Task FadeOutAsync(Color? color = null)
    {
        BeginFade(-1, color);
        await Task.Delay(TimeSpan.FromSeconds(fadeSpeed));
    }

    public async Task FadeInAsync(Color? color = null)
    {
        BeginFade(1, color);
        await Task.Delay(TimeSpan.FromSeconds(fadeSpeed));
    }


}