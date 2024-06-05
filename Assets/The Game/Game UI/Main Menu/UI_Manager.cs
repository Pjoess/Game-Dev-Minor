using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UI_Manager : MonoBehaviour
{
    #region References

    // UI and audio references
    public AudioSource buttonClick;
    public GameObject choicePanel;
    public GameObject MainMenuPanel;
    public GameObject SkipVideoText;
    public VideoPlayer introVideo;
    public static bool videoEnded = false;
    private float originalVolume;

    #endregion

    #region Unity Callbacks

    void Awake()
    {
        LoadVolume(); // Load volume settings
    }

    void Update()
    {
        // Check for skipping video
        if (Input.GetKeyDown(KeyCode.E))
        {
            SkipVideo();
        }
    }

    #endregion

    #region Volume Control

    // Load volume settings from PlayerPrefs
    private static void LoadVolume()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        }

        if (!PlayerPrefs.HasKey("isMuted"))
        {
            PlayerPrefs.SetInt("isMuted", 0);
        }

        if (PlayerPrefs.GetInt("isMuted") == 1)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        }
    }

    // Mute the sound
    public void MuteSound()
    {
        originalVolume = AudioListener.volume;
        AudioListener.volume = 0;
    }

    // Restore the sound to its original volume
    public void RestoreSound()
    {
        AudioListener.volume = originalVolume;
    }

    #endregion

    #region Game Flow

    // Start playing the game
    public void PlayGame()
    {
        MainMenuPanel.SetActive(false);

        // If there is an intro video, play it
        if (introVideo != null)
        {
            MuteSound();
            introVideo.gameObject.SetActive(true);

            if (SkipVideoText != null)
            {
                SkipVideoText.SetActive(true);
            }

            introVideo.Play();
            introVideo.loopPointReached += OnIntroVideoEnded;
        }
        else
        {
            // If no intro video, display choice panel
            introVideo.gameObject.SetActive(false);
            DisplayChoicePanel();
        }
    }

    // Handle intro video end event
    void OnIntroVideoEnded(VideoPlayer vp)
    {
        introVideo.loopPointReached -= OnIntroVideoEnded;
        introVideo.gameObject.SetActive(false); // Disable the video

        if (SkipVideoText != null)
        {
            SkipVideoText.SetActive(false);
        }

        videoEnded = true; // Mark the video as ended
        RestoreSound();
        DisplayChoicePanel();
    }

    // Display the choice panel
    void DisplayChoicePanel()
    {
        choicePanel.SetActive(true);
    }

    // Skip the intro video
    void SkipVideo()
    {
        if (introVideo != null && introVideo.isPlaying)
        {
            introVideo.time = introVideo.length;
            OnIntroVideoEnded(introVideo); // Manually trigger the end event
        }
    }

    // Load a level
    public void PlayLevel()
    {
        SceneManager.LoadSceneAsync(2);
    }

    // Load the tutorial
    public void PlayTutorial()
    {
        SceneManager.LoadSceneAsync(1);
    }

    // Return to the main menu
    public void ToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    // Check if the video has ended
    public bool VideoEnded()
    {
        return videoEnded;
    }

    // Restart the current level
    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(2);
    }

    // Go back to the main menu from the choice panel
    public void ChoiceToMainMenu()
    {
        choicePanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    // Quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion

    #region Audio

    // Play button click sound
    public void PlayClickSound()
    {
        if (buttonClick != null) buttonClick.Play();
    }

    #endregion
}
