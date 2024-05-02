using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private PlayerInput input;
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    private int index = 0;
    public string[] lines;
    public bool isActive = false;
    Coroutine coroutine;

    void Start(){
        instance = this;
        input = FindObjectOfType<Player_Manager>().GetComponent<PlayerInput>();
        Debug.Log("Start");
        Dialogue.ChangeLines += ChangeLine;
        StartDialogue();
    }

    void ChangeLine(string[] lines){
        if(coroutine!=null){
            StopCoroutine(coroutine);
        }
        this.lines = lines;
        StartDialogue();
    }
    void Awake(){
        textComponent.text = string.Empty;
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
        Debug.Log("Start Dialogue");
        input.SwitchCurrentActionMap("UI");
        isActive = true;
        gameObject.SetActive(true);
        textComponent.text = string.Empty;
        index = 0;
        coroutine = StartCoroutine(TextCoroutine());
    }

    IEnumerator TextCoroutine(){
        yield return new WaitForSeconds(textSpeed);
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
            input.SwitchCurrentActionMap("PlayerFull");
            isActive = false;
            index = 0;
        }
    }
}
