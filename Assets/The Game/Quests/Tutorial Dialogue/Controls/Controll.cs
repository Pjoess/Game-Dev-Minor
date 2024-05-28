using UnityEngine;
using UnityEngine.InputSystem;


public class Controll : MonoBehaviour 
{
    public KeybindingSO[] keybindingSOs;
    public static event System.Action<KeybindingSO[]> ChangeControlls;

    void Awake(){
        ChangeControlls = null;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            ChangeControlls(keybindingSOs);
        }
    }

}
