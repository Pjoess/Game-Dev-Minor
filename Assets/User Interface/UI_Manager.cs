using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public AudioSource buttonClick;

    void Awake(){
        buttonClick = GetComponent<AudioSource>();
    }

    void Start(){
        // Check the number of monitors connected.
        if (Display.displays.Length > 1)
        {
            // Activate the display 1 (second monitor connected to the system).
            Display.displays[1].Activate();
        }
    }

    public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
    }

    public void ToMainMenu(){
        SceneManager.LoadSceneAsync(0);
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
