using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject firstSelected;
    private PlayerInput input;
    
    private void Start()
    {
        input = FindObjectOfType<Player_Manager>().GetComponent<PlayerInput>();
    }

    public void EnablePauseCanvas(){
        if(Player_Manager.isPaused){
            pauseMenu.SetActive(true);
            // input.SwitchCurrentActionMap("UI");
            if(input.currentControlScheme.Equals("Gamepad")) EventSystem.current.SetSelectedGameObject(firstSelected);
            Debug.Log(input.currentActionMap);
        } else {
            pauseMenu.SetActive(false);
            if(DialogueManager.instance != null)
            {
                if(!DialogueManager.instance.isActive)
                {
                    // input.SwitchCurrentActionMap("Player");
                }
            }
            // else input.SwitchCurrentActionMap("Player");
            Debug.Log(input.currentActionMap);
        }
    }
}
