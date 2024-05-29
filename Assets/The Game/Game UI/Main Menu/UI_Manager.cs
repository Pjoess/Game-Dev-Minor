using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UI_Manager : MonoBehaviour
{
    public AudioSource buttonClick;
    public GameObject choicePanel;
    public GameObject MainMenuPanel;
    public VideoPlayer introVideo;
    private bool videoEnded = false;

    void Awake()
    {
        LoadVolume();
    }

    void OnEnable()
    {
        ResetVideo();
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
        choicePanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void ToMainMenu(){
        SceneManager.LoadSceneAsync(0);
    }

    void ResetVideo()
    {
        videoEnded = false;
        if(introVideo != null){
            introVideo.Stop();
            introVideo.time = 0;
        }
    }

    void SkipVideo()
    {
        if (introVideo.isPlaying)
        {
            introVideo.time = introVideo.length;
            videoEnded = true;
        }
    }

    public void PlayLevel()
    {
        choicePanel.SetActive(false);

        if (introVideo != null)
        {
            if (videoEnded)
            {
                ResetVideo();
            }
            introVideo.Play();
        }
        introVideo.loopPointReached += OnLevelVideoEnded;
    }

    public void PlayTutorial()
    {
        choicePanel.SetActive(false);

        if (introVideo != null)
        {
            if (videoEnded)
            {
                ResetVideo();
            }
            introVideo.Play();
        }
        introVideo.loopPointReached += OnTutorialVideoEnded;
    }

    void OnLevelVideoEnded(VideoPlayer vp)
    {
        introVideo.loopPointReached -= OnLevelVideoEnded;

        SceneManager.LoadSceneAsync(2);
    }

    void OnTutorialVideoEnded(VideoPlayer vp)
    {
        introVideo.loopPointReached -= OnTutorialVideoEnded;

        SceneManager.LoadSceneAsync(1);
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

    public void QuitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void PlayClickSound(){
        if(buttonClick != null) buttonClick.Play();
    }
}
