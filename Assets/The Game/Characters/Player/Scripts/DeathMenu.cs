using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    [Header("Game Object References")]
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject deathButton;

    public void EnableDeathCanvas(int healthPoints){
        if(healthPoints <= 0){
            deathPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(deathButton);
            StartCoroutine(WaitSeconds());
        } else {
            deathPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void RestartLevel2()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(0f);
        Time.timeScale = 0;
    }
}