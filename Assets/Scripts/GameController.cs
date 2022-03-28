using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum GameState { Normal, Dialog }
public class GameController : MonoBehaviour
{
    [SerializeField]
    public GameState state = GameState.Normal;
    void Start()
    {

        DialogManager.Instance.OnShowDialog += () =>
        {
            Debug.Log("onShowDialog");
            state = GameState.Dialog;
        };
    }
    void Update()
    {
        if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }
}
