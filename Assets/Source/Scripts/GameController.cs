using System.Collections;
using System.Collections.Generic;
using AdventureWorld.Prueba;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public UiManager uiManager;
    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }

            return _instance;
        }
    }
    
    public bool IsPaused => uiManager.CurrentState != UIState.None;

    public void Start()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        } 
        _instance = this;
        DontDestroyOnLoad(this);
        uiManager = GetComponent<UiManager>();
    }
}