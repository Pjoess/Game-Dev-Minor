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
    private RebindingOperation rebindingOperation;


    private KeybindingSO currentBinding;
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
        SetBinding(button);

        rebindingOperation = currentAction.PerformInteractiveRebinding().WithTargetBinding(currentBinding.compositeNumber).WithBindingGroup("Keyboard&Mouse").WithControlsHavingToMatchPath("<Keyboard>")
            .WithControlsHavingToMatchPath("<Mouse>").WithCancelingThrough("<Keyboard>/escape").OnMatchWaitForAnother(0.1f).OnPotentialMatch(operation => CheckBinding())
            .OnComplete(operation => { RebindComplete(button); }).OnCancel(operation => { RebindCancel(button); });

        button.GetComponentInChildren<TMP_Text>().text = "listening...";
        disableAllButtons();
        rebindingOperation.Start();
    }

    private void CheckBinding()
    {
        string displayName = rebindingOperation.selectedControl.displayName;
        string shortDisplayName = rebindingOperation.selectedControl.shortDisplayName;

        //foreach (var binding in input.actions.bindings)
        foreach (var control in input.actions.bindings)
        {
            if(control.groups.Contains("Keyboard&Mouse"))
            {
                if (control.ToDisplayString().Equals(displayName) || control.ToDisplayString().Equals(shortDisplayName))
                {
                    rebindingOperation.Cancel();
                    break;
                }
            }
        }
        rebindingOperation.Complete();
    }

    private void UpdateButton(Button button)
    {
        button.GetComponentInChildren<TMP_Text>().text = button.GetComponent<AssignedBinding>().binding.GetBinding(input);
    }

    private void UpdateAllButtons()
    {
        foreach (Button button in allButtons)
        {
            if(!button.name.Equals("Back"))
            {
                button.GetComponentInChildren<TMP_Text>().text = button.GetComponent<AssignedBinding>().binding.GetBinding(input);
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


    private void SetBinding(Button button)
    {
        currentBinding = button.GetComponent<AssignedBinding>().binding;
        SetAction(currentBinding.actionName);
    }
    private void SetAction(string actionName)
    {
        currentAction = input.actions.FindAction(actionName);
    }

}
