using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VictoryScript : MonoBehaviour
{
    #region Game Object References

    [Header("Game Object References")]
    [SerializeField] private GameObject victoryMenuPanel;
    [SerializeField] private GameObject endVideoTexture;
    [SerializeField] private VideoPlayer endVideo;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        // Prepare the video player and set up the event listener for when the video ends
        endVideo.Prepare();
        endVideo.loopPointReached += LoadMainMenu;
    }

    #endregion

    #region Victory Handling

    public void EnableVictoryCanvas(bool quesCompleted)
    {
        if (quesCompleted)
        {
            // Display the victory menu and video, unlock the cursor, and pause the game
            victoryMenuPanel.SetActive(true);
            endVideoTexture.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            PauseAllOtherMusic();
            endVideo.Play();
            FindObjectOfType<Player_Manager>().GetComponent<PlayerInput>().DeactivateInput();
            Time.timeScale = 0;
        }
        else
        {
            // Hide the victory menu and resume the game
            victoryMenuPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    #endregion

    #region Audio Control

    private void PauseAllOtherMusic()
    {
        // Stop all audio sources in the scene
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }
    }

    #endregion

    #region Scene Management

    private void LoadMainMenu(VideoPlayer vp)
    {
        // Restore player input, reset time scale, and load the main menu
        FindObjectOfType<Player_Manager>().GetComponent<PlayerInput>().ActivateInput();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    #endregion
}
