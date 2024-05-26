using UnityEngine;
using UnityEngine.InputSystem;


public class Controll : MonoBehaviour {
    private string name;
    private PlayerInput input;

    private string KeybindName;

    public string TextFormat = "";
    
    public Controll(string name, PlayerInput input){
        this.name = name;
        this.input = input;
        
        TextFormat.Replace("{button}", KeybindName);
    }

    public void UpdateVariables(){
        KeybindName = input.actions[name].GetBindingDisplayString();
        TextFormat.Replace("{button}", KeybindName);
    }
}
