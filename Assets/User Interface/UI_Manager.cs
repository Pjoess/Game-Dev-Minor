using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    void Start(){

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
}
