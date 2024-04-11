using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Quest")]
    private bool questCompleted = false;
    [SerializeField] private TMP_Text questLog;

    [Header("Stage")]
    [SerializeField] QuestStage[] questStages;
    private int currentStage = 0;

    [Header("Object Script References")]
    [SerializeField] private VictoryScript victoryScript;

    void Awake(){
        victoryScript = FindAnyObjectByType<VictoryScript>();
    }

    void Start()
    {
        questStages[currentStage].StartStage();
        UpdateLog(questStages[currentStage].questLogText);
    }

    void Update()
    {
        if(!questCompleted)
        {
            UpdateLog(questStages[currentStage].questLogText);
            CheckQuestStageComplete();
        }
        
    }

    private void CheckQuestStageComplete()
    {
        if (questStages[currentStage].CheckStageCompleted())
        {
            if (currentStage < questStages.Length - 1)
            {
                currentStage++;
                questStages[currentStage].StartStage();
            }
            else
            {
                UpdateLog("Quest Completed");
                questCompleted = true;
                victoryScript.EnableVictoryCanvas(questCompleted);
            }
        }
    }

    private void UpdateLog(string text)
    {
        questLog.text = text;
    }
}

/* The quest has 4 stages:
 * -Enter the village
 * -Get the memory sticks
 * -Go to the castle
 * -The end
*/