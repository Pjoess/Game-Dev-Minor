using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Keybinding", menuName = "ScriptableObjects/Keybinding")]
public class KeybindingSO : ScriptableObject
{
    public string actionName;
    public bool isComposite = false;
    public string compositePartName;
    public int compositeNumber;

    public string GetBinding(PlayerInput playerInput)
    {
        //playerInput.actions.bindings;
        if(!isComposite) return playerInput.actions.FindAction(actionName).GetBindingDisplayString(InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
        else
        {
            var action = playerInput.actions.FindAction(actionName);
            return action.GetBindingDisplayString(compositeNumber, InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
        }

    }
}
// 0 = no composite, so normal key, 1 = w/s/a/d 2 = w 3 = s 4 = a, 5 = d