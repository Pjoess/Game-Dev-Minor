using TMPro;
using UnityEngine;
using System.Collections;

public class QuestManager : MonoBehaviour
{
    public static AudioSource QuestSound;

    [Header("Quest")]
    private bool questCompleted = false;
    [SerializeField] private TMP_Text questLog;
    [SerializeField] private TMP_Text questNotification; // TextMeshPro for displaying the notifications

    [Header("Stage")]
    [SerializeField] QuestStage[] questStages;
    private int currentStage = 0;

    [Header("Object Script References")]
    [SerializeField] private VictoryScript victoryScript;

    public bool isTutorial;

    void Awake()
    {
        victoryScript = FindAnyObjectByType<VictoryScript>();
    }

    void Start()
    {
        // Find the AudioSource with the name "QuestUpdateSound"
        QuestSound = GameObject.Find("QuestUpdateSound")?.GetComponent<AudioSource>();
        if (QuestSound != null)
        {
            questStages[currentStage].StartStage();
            string currentQuestLogText = questStages[currentStage].questLogText;
            UpdateLog(currentQuestLogText, true);
        }
    }

    void Update()
    {
        if (!questCompleted)
        {
            string currentQuestLogText = questStages[currentStage].questLogText;

            // Only update the quest log if it has changed
            if (questLog.text != currentQuestLogText)
            {
                UpdateLog(currentQuestLogText, true);
            }

            // Check quest stage completion only if the quest log has changed
            if (questLog.text == currentQuestLogText)
            {
                CheckQuestStageComplete();
            }
        }
    }

    private void CheckQuestStageComplete()
    {
        if (questStages[currentStage].CheckStageCompleted())
        {
            if (currentStage < questStages.Length - 1)
            {
                currentStage++;
                QuestSound.Play();
                ShowQuestNotification(questStages[currentStage].questLogText);
                questStages[currentStage].StartStage();
            }
            else
            {
                UpdateLog("Quest Completed", true);
                questCompleted = true;
            }

            // Enable quest notification again
            questNotification.gameObject.SetActive(true);
        }
    }

    private void UpdateLog(string text, bool playSound)
    {
        questLog.text = text;
        if (playSound && QuestSound != null)
        {
            QuestSound.Play();
            ShowQuestNotification(text);
        }
    }

    #region Notification Popup
    private void ShowQuestNotification(string text)
    {
        questNotification.text = text;
        questNotification.alpha = 1;
        questNotification.gameObject.SetActive(true); // Enable the quest notification text
        StartCoroutine(FadeOutText(4f, questNotification));
    }

    private IEnumerator FadeOutText(float fadeDuration, TMP_Text text)
    {
        float startAlpha = text.alpha;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            text.alpha = Mathf.Lerp(startAlpha, 0, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }
        text.alpha = 0;
        text.gameObject.SetActive(false); // Disable the quest notification text after fade out
    }
    #endregion
}

/* The quest has 4 stages:
 * -Enter the village
 * -Get the memory sticks
 * -Go to the castle
 * -The end
*/
