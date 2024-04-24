using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    private int index;
    public string[] lines;
    Coroutine coroutine;

    IDialogueTrigger dialogue;


    void Awake(){
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(textComponent.text == lines[index]){
                NextLine();
            }else{
                StopCoroutine(coroutine);
                textComponent.text = lines[index];
            }
        }

    }

    public void StartDialogue(){
        gameObject.SetActive(true);
        textComponent.text = string.Empty;
        index = 0;
        coroutine = StartCoroutine(TextCoroutine());
    }

    IEnumerator TextCoroutine(){
        foreach(char c in lines[index].ToCharArray()){
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine(){
        textComponent.text = string.Empty;
        if(index < lines.Length - 1){
            index++;
            coroutine = StartCoroutine(TextCoroutine());
        }else{
            gameObject.SetActive(false);
        }
    }
}
