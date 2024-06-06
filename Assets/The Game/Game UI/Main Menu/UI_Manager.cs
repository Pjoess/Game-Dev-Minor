using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UI_Manager : MonoBehaviour
{
    #region References

    // UI and audio references
    public AudioSource buttonClick;
    //public GameObject choicePanel;
    public GameObject MainMenuPanel;
    public GameObject SkipVideoText;
    public VideoPlayer introVideo;
    public static bool videoEnded = false;
    private float originalVolume;

    private bool playMainGame = false;

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
        //DisplayChoicePanel();
    }

    // User chooses to play the main game
    public void OnPlayGameSelected()
    {
        //choicePanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        playMainGame = true;
        PlayIntroOrLoadScene();
    }

    // User chooses to play the tutorial
    public void OnPlayTutorialSelected()
    {
        //choicePanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        playMainGame = false;
        PlayIntroOrLoadScene();
    }

    // Play intro video or load the scene directly if no intro
    void PlayIntroOrLoadScene()
    {
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
            LoadScene();
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
        LoadScene();
    }

    // Load the selected scene
    void LoadScene()
    {
        if (playMainGame)
        {
            SceneManager.LoadSceneAsync(2); // Main game scene
        }
        else
        {
            SceneManager.LoadSceneAsync(1); // Tutorial scene
        }
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

    // Return to the main menu
    public void ToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    // Restart the current level
    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(2);
    }

    // Go back to the main menu from the choice panel
    public void ChoiceToMainMenu()
    {
        //choicePanel.SetActive(false);
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
