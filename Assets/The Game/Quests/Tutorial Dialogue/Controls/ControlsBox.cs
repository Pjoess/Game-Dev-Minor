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
    public KeybindingSO[] controlls;
    public string extraText;
    public PlayerInput input;
    private string currentControllScheme;

    void Start(){
        extraText = "";
        textComponent.text = "";
        currentControllScheme = input.currentControlScheme;
        Controll.ChangeControlls += ChangeControlls;
        UpdateText();
    }

    void Update(){
        //check for control scheme change, then UpdateText();

        if(input.currentControlScheme != currentControllScheme){
            currentControllScheme = input.currentControlScheme;
            UpdateText();
        }
    }

    void ChangeControlls(KeybindingSO[] keybinds, string extraText){
        this.controlls = keybinds;
        this.extraText = extraText;
        UpdateText();
    }

    void UpdateText(){
        //set text to null and add all controlls keybinds in.
        textComponent.text = "";
        foreach(var control in controlls){
            if(control.isComposite){
                textComponent.text += control.actionName + " " + control.compositePartName + " " + control.GetBinding(input) + "\n";
            }else{
                textComponent.text += control.actionName + " " + control.GetBinding(input) + "\n";
            }
        }
        textComponent.text += extraText;
    }
}
