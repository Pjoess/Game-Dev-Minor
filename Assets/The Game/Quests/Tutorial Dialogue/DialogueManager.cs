using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public List<Dialogue> dialogues = new List<Dialogue>();

    private PlayerInput input;
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    private int index = 0;
    public int inputIndex = 0;
    public string[] lines;
    public bool isActive = false;
    Coroutine coroutine;

    public CinemachineVirtualCamera virtualCamera;

    void Start(){
        instance = this;
        input = FindObjectOfType<Player_Manager>().GetComponent<PlayerInput>();
        // input.currentActionMap.FindAction("Move").Disable();
        // input.currentActionMap.FindAction("MoveCamera").Disable();
        // input.currentActionMap.FindAction("Zoom").Disable();
        // input.currentActionMap.FindAction("ToggleBuddyAttack").Disable();
        // input.currentActionMap.FindAction("Sprint").Disable();
        // input.currentActionMap.FindAction("Attack").Disable();
        // input.currentActionMap.FindAction("Dodge").Disable();

        // input.actions["Move"].Disable();
        // input.actions["MoveCamera"].Disable();
        // input.actions["Zoom"].Disable();
        // input.actions["ToggleBuddyAttack"].Disable();
        // input.actions["Sprint"].Disable();
        // input.actions["Attack"].Disable();
        // input.actions["Dodge"].Disable();
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
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        
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

    void SetInputActive(int inputIndex){
        if(inputIndex <= dialogues.Count -1){
            switch (inputIndex)
            {
                case 0:
                    input.currentActionMap.FindAction("Move").Enable();
                    break;
                case 1:
                    input.actions["OnSprint"].Enable();
                    break;
                case 2:
                    input.actions["MoveCamera"].Enable();
                    input.actions["Zoom"].Enable();
                    break;
                case 3:
                    input.actions["Attack"].Enable();
                    break;
                case 4:
                    input.actions["Dodge"].Enable();
                    break;
                case 5:
                    input.actions["ToggleBuddyAttack"].Enable();
                    break;
            }
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
            // SetInputActive(inputIndex);
            inputIndex++;
            isActive = false;
            index = 0;
        }
    }
}
