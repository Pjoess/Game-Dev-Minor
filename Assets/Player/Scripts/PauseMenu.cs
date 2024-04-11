using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    void Awake(){
        pauseMenu = GameObject.Find("PauseMenuCanvas"); // Assuming the Pause Menu Canvas has this name
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void EnablePauseCanvas(){
        if(Player.isPaused){
            pauseMenu.SetActive(true);
        } else {
            pauseMenu.SetActive(false);
        }
    }
}
