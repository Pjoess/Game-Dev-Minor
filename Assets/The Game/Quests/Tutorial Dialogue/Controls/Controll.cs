using UnityEngine;
using UnityEngine.InputSystem;


public class Controll : MonoBehaviour 
{
    public KeybindingSO[] keybindingSOs;
    public string extraText;
    public static event System.Action<KeybindingSO[], string> ChangeControlls;

    void Awake(){
        ChangeControlls = null;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            ChangeControlls(keybindingSOs, extraText);
        }
    }

}
