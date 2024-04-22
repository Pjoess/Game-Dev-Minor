using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    public string[] lines;

    private int index;
    Coroutine coroutine;

    void Start(){
        textComponent.text = string.Empty;
        StartDialogue();
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

    void StartDialogue(){
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
