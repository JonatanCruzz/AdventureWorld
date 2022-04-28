using System.Collections;
using System.Collections.Generic;
using AdventureWorld.Prueba;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class GameController : MonoBehaviour
{
    public UiManager uiManager;
    private static GameController _instance;

    public Transform player;
    public ProCamera2DRooms camera2DRooms;

    public AudioSource audio;

    [System.Serializable]
    public class Areas
    {
        public string area;
        public AudioClip music;
    }

    public List<Areas> bg_music_areas = new List<Areas>();



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
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this);
        uiManager = GetComponent<UiManager>();

        UpdateMusic();
    }

    public void UpdateMusic()
    {
        
        foreach (var ar in camera2DRooms.Rooms)
        {
            foreach (var area in bg_music_areas)
            {
                if (area.area == ar.ID)
                {
                    if (player.localPosition.x > (ar.Dimensions.x - (ar.Dimensions.width / 2)))
                    {
                        if (player.localPosition.x < (ar.Dimensions.x + (ar.Dimensions.width / 2)))
                        {
                            if (player.localPosition.y > (ar.Dimensions.y - (ar.Dimensions.height / 2)))
                            {
                                if (player.localPosition.y < (ar.Dimensions.y + (ar.Dimensions.height / 2)))
                                {
                                    if (audio.clip == area.music)
                                        return;

                                    audio.Stop();
                                    audio.clip = area.music;
                                    audio.Play();
                                }
                            }  
                        }
                    }
                }
            }
        }
    }
}