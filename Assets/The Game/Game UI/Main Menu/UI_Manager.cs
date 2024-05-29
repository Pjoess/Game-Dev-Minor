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
        // Get the main audio listener in the scene
        audioListener = FindObjectOfType<AudioListener>();

        // Check if this is a level or tutorial video
        if (SceneManager.GetActiveScene().buildIndex == 1) // Assuming tutorial scene index is 1
        {
            // Subscribe to the tutorial video's completion event
            introVideo.loopPointReached += OnTutorialVideoEnded;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2) // Assuming level scene index is 2
        {
            // Subscribe to the level video's completion event
            introVideo.loopPointReached += OnLevelVideoEnded;
        }
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
    
    // Set up the event handler for playing levels
    introVideo.loopPointReached += OnLevelVideoEnded;
    }

    public void PlayTutorial()
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
    
    // Set up the event handler for playing tutorials
    introVideo.loopPointReached += OnTutorialVideoEnded;
    }

    void OnLevelVideoEnded(VideoPlayer vp)
    {
        introVideo.loopPointReached -= OnLevelVideoEnded;
        if (audioListener != null)
            audioListener.enabled = true;

        // Load the level scene (assuming it's scene index 2)
        SceneManager.LoadSceneAsync(2);
    }

    void OnTutorialVideoEnded(VideoPlayer vp)
    {
        introVideo.loopPointReached -= OnTutorialVideoEnded;
        if (audioListener != null)
            audioListener.enabled = true;

        // Load the tutorial scene (assuming it's scene index 1)
        SceneManager.LoadSceneAsync(1);
    }

    public bool VideoEnded()
    {
        return videoEnded;
    }

    public void RestartLevel(){
        SceneManager.LoadSceneAsync(2);
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
