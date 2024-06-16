using UnityEngine;

public class Control : MonoBehaviour 
{
    public KeybindingSO[] keybindingSOs;
    public KeybindingSO[] controllerKeybindingSOs;
    public string extraText;
    public bool separateControls = false;
    public static event System.Action<KeybindingSO[], string> ChangeControls;
    public static event System.Action<KeybindingSO[], KeybindingSO[], string> ChangeSeparateControls;

    void Awake(){
        ChangeControls = null;
        ChangeSeparateControls = null;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            if(separateControls) ChangeSeparateControls(keybindingSOs, controllerKeybindingSOs, extraText);
            else ChangeControls(keybindingSOs, extraText);
        }
    }

}
