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
    private bool videoEnded = false;

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

    void OnEnable()
    {
        // Reset video when the GameObject is enabled
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
        choicePanel.SetActive(false);
        if (audioListener != null)
            audioListener.enabled = false;

        // Check if introVideo is not null before trying to use it
        if (introVideo != null)
        {
            // If video has ended, reset it back to false and start from the beginning
            if (videoEnded)
            {
                ResetVideo();
            }

            // Play the video
            introVideo.Play();
        }
    }

    void ResetVideo()
    {
        videoEnded = false;
        introVideo.Stop();
        introVideo.time = 0;
    }

    void SkipVideo()
    {
        if (introVideo.isPlaying)
        {
            introVideo.time = introVideo.length;
            videoEnded = true;
        }
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        introVideo.loopPointReached -= OnVideoEnded;
        if (audioListener != null)
            audioListener.enabled = true;
        
        SceneManager.LoadSceneAsync(2);
    }

    public bool VideoEnded()
    {
        return videoEnded;
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
