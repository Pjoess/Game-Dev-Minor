using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_UI : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(0);
        }
    }
}
