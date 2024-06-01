using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UI_Manager : MonoBehaviour
{
    public AudioSource buttonClick;
    public GameObject choicePanel;
    public GameObject MainMenuPanel;
    public VideoPlayer introVideo;
    public static bool videoEnded = false;

    void Awake()
    {
        LoadVolume(); // for sound
    }

    void Update()
    {
        // Skip video
        if (Input.GetKeyDown(KeyCode.E))
        {
            SkipVideo();
        }
    }

    private static void LoadVolume()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        }
    }

    public void PlayGame()
    {
        MainMenuPanel.SetActive(false);

        if (introVideo != null && !videoEnded) // Video Plays only one time per Startup of the Game
        {
            introVideo.gameObject.SetActive(true);
            introVideo.Play();
            introVideo.loopPointReached += OnIntroVideoEnded;
        }
        else
        {
            introVideo.gameObject.SetActive(false);
            DisplayChoicePanel();
        }
    }

    void OnIntroVideoEnded(VideoPlayer vp)
    {
        introVideo.loopPointReached -= OnIntroVideoEnded;
        introVideo.gameObject.SetActive(false); // Disable the video
        videoEnded = true; // Mark the video as ended
        DisplayChoicePanel();
    }

    void DisplayChoicePanel()
    {
        choicePanel.SetActive(true);
    }

    void SkipVideo()
    {
        if (introVideo != null && introVideo.isPlaying)
        {
            introVideo.time = introVideo.length;
            OnIntroVideoEnded(introVideo); // Manually trigger the end event
        }
    }

    public void PlayLevel()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayTutorial()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public bool VideoEnded()
    {
        return videoEnded;
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void ChoiceToMainMenu()
    {
        choicePanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void PlayClickSound()
    {
        if (buttonClick != null) buttonClick.Play();
    }
}
