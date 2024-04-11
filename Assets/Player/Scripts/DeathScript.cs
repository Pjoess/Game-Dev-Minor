using System.Collections;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    [Header("Game Object References")]
    [SerializeField] private GameObject deathPanel;

    public void EnableDeathCanvas(int healthPoints){
        if(healthPoints <= 0){
            deathPanel.SetActive(true);
            StartCoroutine(WaitSeconds());
        } else {
            deathPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    IEnumerator WaitSeconds()
    {
        //victoryMedievalSound.Play();
        PauseAllOtherMusic();
        yield return new WaitForSeconds(0f);
        //victoryCongratulations.Play();
        Time.timeScale = 0;
    }

    void PauseAllOtherMusic()
        {
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in allAudioSources)
            {
                audioSource.Pause();
            }
        }

}