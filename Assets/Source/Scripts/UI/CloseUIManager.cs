using System;
using System.Collections;
using System.Collections.Generic;
using AdventureWorld.Prueba;
using UnityEngine;

public class CloseUIManager : MonoBehaviour
{
    
    public GameObject canvasPrefab;
    private CloseUI ui;
    void Start()
    {
        ui = Instantiate(canvasPrefab).GetComponent<CloseUI>();
        this.HideCanvas();
    }

    
    public void ShowCanvas()
    {
        
        ui.canvas.enabled = true;
    }
    
    public void HideCanvas()
    {
        ui.canvas.enabled = false;
    }

}
