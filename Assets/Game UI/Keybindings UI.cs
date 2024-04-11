using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class KeybindingsUI : MonoBehaviour
{
    private PlayerInput input;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private InputAction currentAction;

    public Button[] allButtons;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        allButtons = FindObjectsByType<Button>(FindObjectsSortMode.InstanceID);

        if (PlayerPrefs.HasKey("controls"))
        {
            input.actions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("controls"));
        }

        UpdateAllButtons();
    }

    public void RebindButton(Button button)
    {
        SetAction(button.name);
        rebindingOperation = currentAction.PerformInteractiveRebinding().WithBindingGroup("Keyboard&Mouse").WithControlsHavingToMatchPath("<Keyboard>").WithControlsHavingToMatchPath("<Mouse>").WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f).OnPotentialMatch(operation => CheckBinding(button.name)).OnComplete(operation => { RebindComplete(button); }).OnCancel(operation => { RebindCancel(button); });

        button.GetComponentInChildren<TMP_Text>().text = "listening...";
        disableAllButtons();
        rebindingOperation.Start();
    }

    private void CheckBinding(string actionName)
    {
        foreach (Button button in allButtons) 
        {
            if (!button.name.Equals("Back") && !button.name.Equals(actionName))
            {
                string checkAgainst = (rebindingOperation.selectedControl.shortDisplayName == null) ? rebindingOperation.selectedControl.displayName :  rebindingOperation.selectedControl.shortDisplayName;
                string key = input.actions.FindAction(button.name).GetBindingDisplayString();
                if (checkAgainst.Equals(key))
                {
                    rebindingOperation.Cancel();
                }
            }
        }
        rebindingOperation.Complete();
    }

    private void UpdateButton(Button button)
    {
        button.GetComponentInChildren<TMP_Text>().text = currentAction.GetBindingDisplayString();
    }

    private void UpdateAllButtons()
    {
        foreach (Button button in allButtons)
        {
            if(!button.name.Equals("Back"))
            {
                button.GetComponentInChildren<TMP_Text>().text = input.actions.FindAction(button.name).GetBindingDisplayString();
            }
        }
    }

    private void RebindComplete(Button button)
    {
        rebindingOperation.Dispose();
        UpdateButton(button);
        PlayerPrefs.SetString("controls", input.actions.SaveBindingOverridesAsJson());
        enableAllButtons();
    }

    private void RebindCancel(Button button)
    {
        rebindingOperation.Dispose();
        UpdateButton(button);
        enableAllButtons();
    }

    private void disableAllButtons()
    {
        foreach (Button button in allButtons)
        {
            button.interactable = false;
        }
    }
    private void enableAllButtons()
    {
        foreach (Button button in allButtons)
        {
            button.interactable = true;
        }
    }

    private void SetAction(string actionName)
    {
        currentAction = input.actions.FindAction(actionName);
    }

}
