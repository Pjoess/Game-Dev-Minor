using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public List<Dialogue> dialogues = new();
    private PlayerInput input;
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    private int index = 0;
    public int inputIndex = 0;
    public string[] lines;
    public bool isActive = false;
    Coroutine coroutine;
    public ShowSlimes showSlimes;
    public bool isLastOne = false;
    public CinemachineVirtualCamera virtualCamera;

    public GameObject child;

    void Awake(){
        textComponent.text = string.Empty;
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        // Dialogue.ChangeLines += null;
        instance = null;
        coroutine = null;
        // input = null;
        index = 0;
        inputIndex = 0;
        isLastOne = false;
        // DontDestroyOnLoad(this.gameObject);
    }
    void Start(){
        Dialogue.ChangeLines += ChangeLine;
        child.SetActive(false);
        instance = this;
        input = FindObjectOfType<Player_Manager>().GetComponent<PlayerInput>();
        Debug.Log("Start");
        
        StartDialogue();
    }

    void ChangeLine(string[] lines){
        if(coroutine!=null){
            StopCoroutine(coroutine);
        }
        this.lines = lines;
        StartDialogue();
    }


    void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(textComponent.text == lines[index]){
                NextLine();
            }else{
                if(coroutine!=null){
                    StopCoroutine(coroutine);
                }
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue(){
        Debug.Log("Start Dialogue");
        input.SwitchCurrentActionMap("UI");
        isActive = true;
        child.SetActive(true);
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
        // coroutine = null;
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
            child.SetActive(false); 
            input.SwitchCurrentActionMap("Player");
            inputIndex++;
            isActive = false;
            index = 0;
            coroutine = null;
            if(isLastOne){
                showSlimes.ToggleSlimes();
            }
        }
    }
}
