using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VictoryScript : MonoBehaviour
{
    [Header("Game Object References")]
    [SerializeField] private GameObject victoryMenuPanel;
    [SerializeField] private VideoPlayer endVideo;

    public void EnableVictoryCanvas(bool quesCompleted)
    {
        if(quesCompleted == true)
        {
            victoryMenuPanel.SetActive(true);
            //EventSystem.current.SetSelectedGameObject(victoryButton);
            PauseAllOtherMusic();
            endVideo.Play();
            FindObjectOfType<Player_Manager>().GetComponent<PlayerInput>().DeactivateInput();
            Time.timeScale = 0;
        } 
        else 
        {
            victoryMenuPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void PauseAllOtherMusic()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }
    }

    private void Start()
    {
        endVideo.Prepare();
        endVideo.loopPointReached += LoadMainMenu;
    }

    private void LoadMainMenu(VideoPlayer vp)
    {
        FindObjectOfType<Player_Manager>().GetComponent<PlayerInput>().ActivateInput();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
