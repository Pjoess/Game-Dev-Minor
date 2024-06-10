using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
// using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public List<Dialogue> dialogues = new();
    private PlayerInput input;
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI nameComponent;
    public Image charImage;
    public Sprite buddySprite, arthurSprite;
    public float textSpeed;
    private int index = 0;
    public int inputIndex = 0;
    public DialogueLine[] lines;
    public bool isActive = false;
    Coroutine coroutine;
    public ShowSlimes showSlimes;
    public bool isLastOne = false;
    public CinemachineVirtualCamera virtualCamera;

    public SpriteRenderer icon;
    public Sprite image;

    public bool canMove = false;

    public GameObject child;

    public AudioSource audioSource;

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
        
        if(lines.Length > 0) StartDialogue();
    }

    void ChangeLine(DialogueLine[] lines){
        if(coroutine!=null){
            StopCoroutine(coroutine);
            coroutine = null;
        }
        this.lines = lines;
        StartDialogue();
    }

    public void ChangeIcon(Sprite texture){
        image = texture;
    }


    void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(textComponent.text == lines[index].line){
                NextLine();
            }else{
                if(coroutine!=null){
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
                textComponent.text = lines[index].line;
            }
        }
    }

    public void StartDialogue(){
        if(icon!=null){
            icon.sprite = image;
        }
        Debug.Log("Start Dialogue");
        if(!canMove){
            input.DeactivateInput();
        }
        
        isActive = true;
        child.SetActive(true);
        textComponent.text = string.Empty;
        index = 0;
        ChangeName(lines[index].character);
        coroutine = StartCoroutine(TextCoroutine());
    }

    IEnumerator TextCoroutine(){
        yield return new WaitForSeconds(textSpeed);
        
        if(lines[index].audioClip!=null){
            audioSource.clip = lines[index].audioClip;
            audioSource.Play();
        }
        foreach(char c in lines[index].line.ToCharArray()){
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        // coroutine = null;
        while(audioSource.isPlaying){
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        coroutine = null;
        
        NextLine();
    }

    private void ChangeName(DialogueLine.DialogueCharacter character)
    {
        switch (character)
        {
            case DialogueLine.DialogueCharacter.BUDDY:
                nameComponent.text = "Buddy";
                charImage.sprite = buddySprite;
                break;

            case DialogueLine.DialogueCharacter.ARTHUR:
                nameComponent.text = "Arthur";
                charImage.sprite = arthurSprite;
                break;

            case DialogueLine.DialogueCharacter.KING:
                nameComponent.text = "King";
                break;
        }
    }

    void NextLine(){
        textComponent.text = string.Empty;
        if(index < lines.Length - 1){
            index++;
            if(isLastOne && index == 2){
                showSlimes.ToggleSlimes();
            }
            audioSource.Stop();
            ChangeName(lines[index].character);
            coroutine = StartCoroutine(TextCoroutine());
        }else{
            child.SetActive(false); 
            if(!canMove){
                input.ActivateInput();
            }
            // input.SwitchCurrentActionMap("Player");
            inputIndex++;
            isActive = false;
            index = 0;
            coroutine = null;
            // if(isLastOne){
            //     showSlimes.ToggleSlimes();
            // }
        }
    }
}
