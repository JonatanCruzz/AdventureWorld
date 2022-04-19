using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DialogueSequenceManager : MonoBehaviour
{
   
    public void Message(string message)
    {
        Sequencer.Message(message);
    }
}
