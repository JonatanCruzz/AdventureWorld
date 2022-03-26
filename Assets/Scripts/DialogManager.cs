using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogManager : MonoBehaviour
{

    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSeconds;

    public event Action OnShowDialog;
    public event Action OnSCloseDialog;

    public static DialogManager Instance { get; private set; } //static function for any class we want
    private void Awake(){
        Instance = this;
    }
    Dialog dialog;
    int currentLine = 0;
    bool isTyping;

    public IEnumerator ShowDialog(Dialog dialog){
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();

        this.dialog = dialog;

        dialogBox.SetActive(true);

        StartCoroutine(TypeDialog(dialog.Lines[0])); //first line of dialog
    }

    public void HandleUpdate(){ //this function show the next dialog...
        if(Input.GetKeyDown(KeyCode.E) && !isTyping){
            ++currentLine;

            if(currentLine < dialog.Lines.Count){

                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }else{
                currentLine = 0;

                dialogBox.SetActive(false);

                OnSCloseDialog?.Invoke();
            }
        }
    }

    
    public IEnumerator TypeDialog(string line) //before actually completing the current dialog
    { 
        isTyping = true;

        dialogText.text = ""; //set dialog empty string

        foreach(var letter in line.ToCharArray()){ //basically, looping for each letter of the dialog

            dialogText.text += letter; //adding 1by1 to the text

            yield return new WaitForSeconds(1f / lettersPerSeconds); //returning each letter
        }

        isTyping = false;
    }
}
