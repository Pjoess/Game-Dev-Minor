using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UI_Manager : MonoBehaviour
{
    public AudioSource buttonClick;
    public GameObject choicePanel;
    public GameObject MainMenuPanel;
    public VideoPlayer introVideo;
    private AudioListener audioListener;

    void Awake()
    {
        LoadVolume();
    }

    void Start()
    {
        // Subscribe to the video's completion event
        introVideo.loopPointReached += OnVideoEnded;

        // Get the main audio listener in the scene
        audioListener = FindObjectOfType<AudioListener>();
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
            AudioListener.volume = AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        }
    }

    public void PlayGame(){
        choicePanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void ToMainMenu(){
        SceneManager.LoadSceneAsync(0);
    }

    public void PlayLevel()
    {
        // Mute all audio when video starts playing
        if (audioListener != null)
            audioListener.enabled = false;

        // Play the video from the beginning
        introVideo.Stop();
        introVideo.time = 0;
        introVideo.Play();
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        // Unsubscribe from the event to avoid multiple calls
        introVideo.loopPointReached -= OnVideoEnded;

        // Unmute audio when video ends
        // if (audioListener != null)
        //     audioListener.enabled = true;

        // Load the scene after the video has ended
        SceneManager.LoadSceneAsync(2);
    }

    public void RestartLevel(){
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayTutorial(){
        SceneManager.LoadSceneAsync(1);
    }

    public void ChoiceToMainMenu(){
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
