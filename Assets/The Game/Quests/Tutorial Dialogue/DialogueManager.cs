using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
// using UnityEditor.Animations;
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

    public bool canMove = false;

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
            coroutine = null;
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
                    coroutine = null;
                }
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue(){
        Debug.Log("Start Dialogue");
        if(!canMove){
            input.SwitchCurrentActionMap("UI");
        }
        
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

    void NextLine(){
        textComponent.text = string.Empty;
        if(index < lines.Length - 1){
            index++;
            coroutine = StartCoroutine(TextCoroutine());
        }else{
            child.SetActive(false); 
            if(!canMove){
                input.SwitchCurrentActionMap("Player");
            }
            // input.SwitchCurrentActionMap("Player");
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
