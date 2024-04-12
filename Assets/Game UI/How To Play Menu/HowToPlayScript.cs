using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HowToPlayScript : MonoBehaviour
{
    [SerializeField] private GameObject closePanel;
    private PlayerInput input;

    [SerializeField] TMP_Text forwardsText;
    [SerializeField] TMP_Text backwardsText;
    [SerializeField] TMP_Text leftText;
    [SerializeField] TMP_Text rightText;

    [SerializeField] TMP_Text attackText;
    [SerializeField] TMP_Text dodgeText;
    [SerializeField] TMP_Text sprintText;

    void Start(){
        closePanel.SetActive(true);
        input = FindObjectOfType<Player>().GetComponent<PlayerInput>();

        forwardsText.text = input.actions.FindAction("Move").GetBindingDisplayString(2);
        backwardsText.text = input.actions.FindAction("Move").GetBindingDisplayString(3);
        leftText.text = input.actions.FindAction("Move").GetBindingDisplayString(4);
        rightText.text = input.actions.FindAction("Move").GetBindingDisplayString(5);

        attackText.text = input.actions.FindAction("Attack").GetBindingDisplayString();
        dodgeText.text = input.actions.FindAction("Dodge").GetBindingDisplayString();
        sprintText.text = input.actions.FindAction("Sprint").GetBindingDisplayString();
        input.SwitchCurrentActionMap("UI");
        Debug.Log(input.currentActionMap);
    }

    public void ClosePanel()
    {
        if(closePanel != null){
            closePanel.SetActive(false);
            input.SwitchCurrentActionMap("Player");
            Debug.Log(input.currentActionMap);
            Time.timeScale = 1;
        } else {
            closePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
