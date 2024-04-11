using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    void Awake(){
        pauseMenu = GameObject.Find("PauseMenuCanvas");
    }

    public void EnablePauseCanvas(){
        if(Player.isPaused){
            pauseMenu.SetActive(true);
        } else {
            pauseMenu.SetActive(false);
        }
    }
}
