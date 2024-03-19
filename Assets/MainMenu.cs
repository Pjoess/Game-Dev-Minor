using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/* --- notes ---
This sciript is called in the Play Button under "On Click ()"
How to use this, attach the script on a GameObject so I created one called: MainMenuScriptObject
Then drag and drop the MainMenuScriptObject on the "On Click ()" then select MainMenu then PlayGame
*/
public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame(){
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ToMainMenu(){
        SceneManager.LoadSceneAsync(0);
    }
}
