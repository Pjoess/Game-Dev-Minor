using System.Collections;
using UnityEngine;

public class VictoryScript : MonoBehaviour
{
    [Header("Game Object References")]
    [SerializeField] private GameObject VictoryMenuPanel;

    [Header("Audio Source References")]
    [SerializeField] private AudioSource victoryMedievalSound;
    [SerializeField] private AudioSource victoryCongratulations;

    public void EnableVictoryCanvas(bool quesCompleted){
        if(quesCompleted == true){
            VictoryMenuPanel.SetActive(true);
            StartCoroutine(WaitSeconds());
        } else {
            VictoryMenuPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    IEnumerator WaitSeconds()
    {
        PauseAllOtherMusic();
        victoryMedievalSound.Play();
        yield return new WaitForSeconds(4f);
        victoryCongratulations.Play();
        Time.timeScale = 0;
    }

    void PauseAllOtherMusic()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != victoryMedievalSound && audioSource != victoryCongratulations)
            {
                audioSource.Stop();
            }
        }
    }
}
