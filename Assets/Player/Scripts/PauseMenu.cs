using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private PlayerInput input;
    private void Start()
    {
        input = FindObjectOfType<Player>().GetComponent<PlayerInput>();
    }

    public void EnablePauseCanvas(){
        if(Player.isPaused){
            pauseMenu.SetActive(true);
            input.SwitchCurrentActionMap("UI");
            Debug.Log(input.currentActionMap);
        } else {
            pauseMenu.SetActive(false);
            input.SwitchCurrentActionMap("Player");
            Debug.Log(input.currentActionMap);
        }
    }
}
