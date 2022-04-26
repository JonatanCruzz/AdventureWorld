using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackscreen : MonoBehaviour
{
    public Image img;
    public StartGame sg;
    public void Toggle()
    {
        img.enabled = !img.enabled;
    }

    public void startGa()
    {
        sg.StartG();
    }
}
