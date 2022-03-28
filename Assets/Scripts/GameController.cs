using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{Dialog}
public class GameController : MonoBehaviour
{ 
    GameState state;
   void Start()
    {
       DialogManager.Instance.OnShowDialog += () => {
        state = GameState.Dialog;
       } ;
    }
    void Update()
    {
        if(state == GameState.Dialog){
            DialogManager.Instance.HandleUpdate();
        }
    }
}
