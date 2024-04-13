using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UiSelectionScript : MonoBehaviour
{
    private PlayerInput input;
    public GameObject firstButton, toOptions, toKeybind;
    public GameObject main, options, keybindings;

    public Button mainBack, optionsBack, keybindingBack;

    private InputSystemUIInputModule uiModule;
    private InputAction cancel;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        uiModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
        cancel = uiModule.cancel.action;
    }

    // Update is called once per frame
    void Update()
    {
        if(input.currentControlScheme.Equals("Keyboard&Mouse"))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        else if(EventSystem.current.currentSelectedGameObject == null)
        {
            if (main.activeInHierarchy) EventSystem.current.SetSelectedGameObject(firstButton);
            if (options.activeInHierarchy) EventSystem.current.SetSelectedGameObject(toOptions);
            if (keybindings.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(toKeybind);
            }
        }

        if(cancel.WasPressedThisFrame()) Cancel();
    }

    public void ToOptions()
    {
        main.gameObject.SetActive(false);
        keybindings.gameObject.SetActive(false);
        options.gameObject.SetActive(true);

        if (!input.currentControlScheme.Equals("Keyboard&Mouse"))
        {
            EventSystem.current.SetSelectedGameObject(toOptions);
        }
    }

    public void ToMain()
    {
        options.gameObject.SetActive(false);
        main.gameObject.SetActive(true);

        if (!input.currentControlScheme.Equals("Keyboard&Mouse"))
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }

    public void ToKeybind()
    {
        options.gameObject.SetActive(false);
        keybindings.gameObject.SetActive(true);

        if (!input.currentControlScheme.Equals("Keyboard&Mouse"))
        {
            EventSystem.current.SetSelectedGameObject(toKeybind);
        }
    }

    private void Cancel()
    {
        if (main.activeInHierarchy) mainBack.onClick.Invoke();
        if (options.activeInHierarchy) optionsBack.onClick.Invoke();
        if (keybindings.activeInHierarchy) keybindingBack.onClick.Invoke();
    }
}
