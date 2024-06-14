using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu, controlsMenu;
    [SerializeField] private GameObject firstSelected;
    private PlayerInput input;
    
    private void Start()
    {
        input = FindObjectOfType<Player_Manager>().GetComponent<PlayerInput>();
    }

    public void EnablePauseCanvas(){
        if(Player_Manager.isPaused){
            pauseMenu.SetActive(true);
            input.SwitchCurrentActionMap("UI");
            if(input.currentControlScheme.Equals("Gamepad")) EventSystem.current.SetSelectedGameObject(firstSelected);
            Debug.Log(input.currentActionMap);
        } else {
            pauseMenu.SetActive(false);
            controlsMenu.SetActive(false);
            input.SwitchCurrentActionMap("Player");
            Debug.Log(input.currentActionMap);
        }
    }
}
