using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsBox : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public KeybindingSO[] controls;
    public KeybindingSO[] controllerControls;
    public string extraText;
    public PlayerInput input;
    private string currentControlScheme;

    void Start(){
        extraText = "";
        textComponent.text = "";
        currentControlScheme = input.currentControlScheme;
        Control.ChangeControls += ChangeControls;
        Control.ChangeSeparateControls += ChangeSeparateControls;
        UpdateText();
    }

    void Update(){
        //check for control scheme change, then UpdateText();
        if(input.currentControlScheme != currentControlScheme){
            currentControlScheme = input.currentControlScheme;
            UpdateText();
        }
    }


    void ChangeSeparateControls(KeybindingSO[] keyboardKeybinds, KeybindingSO[] controllerKeybinds, string extraText)
    {
        this.controls = keyboardKeybinds;
        this.controllerControls = controllerKeybinds;
        this.extraText = extraText;
        UpdateText();
    }

    void ChangeControls(KeybindingSO[] keybinds, string extraText){
        this.controls = keybinds;
        this.controllerControls = keybinds;
        this.extraText = extraText;
        UpdateText();
    }

    void UpdateText(){
        //set text to null and add all controlls keybinds in.
        textComponent.text = "";

        if(input.currentControlScheme.Equals("Gamepad"))
        {
            foreach (var control in controllerControls)
            {
                textComponent.text += control.actionName + " " + control.GetBinding(input) + "\n";
            }
        }
        else
        {
            foreach (var control in controls)
            {
                if (control.isComposite)
                {
                    textComponent.text += control.actionName + " " + control.compositePartName + " " + control.GetBinding(input) + "\n";
                }
                else
                {
                    if (control.GetBinding(input).Equals("Delta"))
                    {
                        textComponent.text += control.actionName + " " + control.compositePartName + " Mouse \n";
                    }
                    else textComponent.text += control.actionName + " " + control.GetBinding(input) + "\n";
                }
            }
        }

        textComponent.text += extraText;
    }
}
