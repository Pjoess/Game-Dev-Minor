using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsBox : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public AllControlls allControlls;
    public PlayerInput playerInput;

    public string currentScheme;
    public List<string> list = new List<string>();

    public Controll[] controlls;

    void Start(){
        textComponent.text = "";
        allControlls = new AllControlls();
        currentScheme = playerInput.currentControlScheme;
    }

    void Update(){
        // if(!currentScheme.Equals(playerInput.currentControlScheme)){
        //     allControlls.Update();
        //     currentScheme = playerInput.currentControlScheme;
        // }
        UpdateText();
    }

    void UpdateText(){
        var action = playerInput.actions["Move"];
        var x = playerInput.actions["Move"].bindings;

        for (int i = 0; i<x.Count; i++){
            Debug.Log(x[i]);
            if(x[i].isComposite){
                
                action.GetBindingDisplayString(x[i]);
            }else if(x[i].isPartOfComposite){
                break;
            }else{
                playerInput.actions["Move"].GetBindingDisplayString();
            }
        }
        // textComponent.text = allControlls.Attack.TextFormat;
        // textComponent.text = playerInput.actions["Move"].GetBindingDisplayString();
    }
}
