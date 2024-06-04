using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public GameObject exitObject;

    // Start is called before the first frame update
    void Start()
    {
        exitObject = GameObject.FindWithTag("Exit");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
