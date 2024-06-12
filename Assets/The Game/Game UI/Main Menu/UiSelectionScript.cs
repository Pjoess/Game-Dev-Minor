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
        // Try to get the PlayerInput component
        if (!TryGetComponent<PlayerInput>(out input))
        {
            Debug.Log("PlayerInput component is missing!");
        }
        else
        {
            Debug.Log("PlayerInput component found.");
        }

        // Try to get the InputSystemUIInputModule from the EventSystem
        if (EventSystem.current != null)
        {
            if (!EventSystem.current.TryGetComponent<InputSystemUIInputModule>(out uiModule))
            {
                Debug.Log("InputSystemUIInputModule component is missing!");
            }
            else
            {
                Debug.Log("InputSystemUIInputModule component found.");
            }
        }
        else
        {
            Debug.Log("EventSystem.current is null!");
        }

        // Check if the cancel action is properly assigned
        if (uiModule != null)
        {
            cancel = uiModule.cancel.action;
            if (cancel == null)
            {
                Debug.Log("Cancel action is missing!");
            }
            else
            {
                Debug.Log("Cancel action found.");
            }
        }

        // Log checks for GameObjects
        if (firstButton == null) Debug.Log("firstButton is not assigned!");
        if (toOptions == null) Debug.Log("toOptions is not assigned!");
        if (toKeybind == null) Debug.Log("toKeybind is not assigned!");
        if (main == null) Debug.Log("main is not assigned!");
        if (options == null) Debug.Log("options is not assigned!");
        if (keybindings == null) Debug.Log("keybindings is not assigned!");

        // Log checks for Buttons
        if (mainBack == null) Debug.Log("mainBack is not assigned!");
        if (optionsBack == null) Debug.Log("optionsBack is not assigned!");
        if (keybindingBack == null) Debug.Log("keybindingBack is not assigned!");
    }

    void Update()
    {
        if (input == null)
        {
            Debug.Log("input is null in Update");
            return;
        }

        if (input.currentControlScheme == null)
        {
            Debug.Log("currentControlScheme is null in Update");
            return;
        }

        if (EventSystem.current == null)
        {
            Debug.Log("EventSystem.current is null in Update");
            return;
        }

        if (input.currentControlScheme.Equals("Keyboard&Mouse"))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        else if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (main == null)
            {
                Debug.Log("main is null in Update");
            }
            else if (main.activeInHierarchy)
            {
                if (firstButton == null)
                {
                    Debug.Log("firstButton is null in Update");
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(firstButton);
                }
            }

            if (options == null)
            {
                Debug.Log("options is null in Update");
            }
            else if (options.activeInHierarchy)
            {
                if (toOptions == null)
                {
                    Debug.Log("toOptions is null in Update");
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(toOptions);
                }
            }

            if (keybindings == null)
            {
                Debug.Log("keybindings is null in Update");
            }
            else if (keybindings.activeInHierarchy)
            {
                if (toKeybind == null)
                {
                    Debug.Log("toKeybind is null in Update");
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(toKeybind);
                }
            }
        }

        if (cancel == null)
        {
            Debug.Log("cancel is null in Update");
            return;
        }

        if (cancel.WasPressedThisFrame()) Cancel();
    }

    public void ToOptions()
    {
        Debug.Log("ToOptions called");

        if (main != null) main.SetActive(false);
        else Debug.Log("main GameObject is null in ToOptions");

        if (keybindings != null) keybindings.SetActive(false);
        else Debug.Log("keybindings GameObject is null in ToOptions");

        if (options != null) options.SetActive(true);
        else Debug.Log("options GameObject is null in ToOptions");

        if (!input.currentControlScheme.Equals("Keyboard&Mouse"))
        {
            if (toOptions != null) EventSystem.current.SetSelectedGameObject(toOptions);
            else Debug.Log("toOptions GameObject is null in ToOptions");
        }
    }

    public void ToMain()
    {
        Debug.Log("ToMain called");

        if (options != null) options.SetActive(false);
        else Debug.Log("options GameObject is null in ToMain");

        if (main != null) main.SetActive(true);
        else Debug.Log("main GameObject is null in ToMain");

        if (!input.currentControlScheme.Equals("Keyboard&Mouse"))
        {
            if (firstButton != null) EventSystem.current.SetSelectedGameObject(firstButton);
            else Debug.Log("firstButton GameObject is null in ToMain");
        }
    }

    public void ToKeybind()
    {
        Debug.Log("ToKeybind called");

        if (options != null) options.SetActive(false);
        else Debug.Log("options GameObject is null in ToKeybind");

        if (keybindings != null) keybindings.SetActive(true);
        else Debug.Log("keybindings GameObject is null in ToKeybind");

        if (!input.currentControlScheme.Equals("Keyboard&Mouse"))
        {
            if (toKeybind != null) EventSystem.current.SetSelectedGameObject(toKeybind);
            else Debug.Log("toKeybind GameObject is null in ToKeybind");
        }
    }

    private void Cancel()
    {
        Debug.Log("Cancel called");

        if (main != null && main.activeInHierarchy) mainBack.onClick.Invoke();
        else if (options != null && options.activeInHierarchy) optionsBack.onClick.Invoke();
        else if (keybindings != null && keybindings.activeInHierarchy) keybindingBack.onClick.Invoke();
        else Debug.Log("Active GameObject or its back button is null in Cancel");
    }
}