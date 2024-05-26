using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsBox : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public AllControlls allControlls;
    public PlayerInput playerInput;

    public string currentScheme;

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
        // textComponent.text = allControlls.Attack.TextFormat;
        textComponent.text = playerInput.actions["Sprint"].GetBindingDisplayString();
    }
}
