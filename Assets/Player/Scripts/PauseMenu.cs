using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public void EnablePauseCanvas(){
        if(Player.isPaused){
            pauseMenu.SetActive(true);
        } else {
            pauseMenu.SetActive(false);
        }
    }
}
