using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public AudioSource buttonClick;
    public GameObject choicePanel;
    public GameObject MainMenuPanel;

    void Awake()
    {
        LoadVolume();
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

    public void PlayLevel(){
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
