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
    [SerializeField] private GameObject endVideoText;

    // Define start and end times for the segments to play (in seconds)
    private double[][] outroSegments = new double[][]
    {
        new double[] { 1.0, 8.0 },
        new double[] { 9.0, 19.0 },
        new double[] { 20.0, 29.0 },
        new double[] { 30.0, 50.0 }
    };

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        // Prepare the video player and set up the event listener for when the video ends
        endVideo.Prepare();
        endVideo.loopPointReached += LoadMainMenu;
    }

    private void Update()
    {
        // Check for skipping video
        if (Input.GetKeyDown(KeyCode.F))
        {
            SkipOutroSegment();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SkipWholeOutroVideo();
        }
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
            endVideoText.SetActive(true);
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

    #region Video Skipping

    // Skip to the next segment of the outro video
    private void SkipOutroSegment()
    {
        if (endVideo != null && endVideo.isPlaying)
        {
            double currentTime = endVideo.time;
            for (int i = 0; i < outroSegments.Length; i++)
            {
                if (currentTime >= outroSegments[i][0] && currentTime < outroSegments[i][1])
                {
                    // If it's the last segment, manually trigger the end event
                    if (i == outroSegments.Length - 1)
                    {
                        endVideo.time = endVideo.length;
                        LoadMainMenu(endVideo);
                    }
                    else
                    {
                        endVideo.time = outroSegments[i + 1][0];
                    }
                    break;
                }
            }
        }
    }

    // Skip the whole outro video
    private void SkipWholeOutroVideo()
    {
        if (endVideo != null && endVideo.isPlaying)
        {
            endVideo.time = endVideo.length;
            LoadMainMenu(endVideo); // Manually trigger the end event
        }
    }
    #endregion
}
