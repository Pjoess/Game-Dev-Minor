using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public AudioSource buttonClick;

    void Awake(){
        buttonClick = GetComponent<AudioSource>();
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
