using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool isTutorial;

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
                if(!isTutorial)
                {
                    //victoryScript.EnableVictoryCanvas(questCompleted);
                    SceneManager.LoadScene(3);
                    Debug.Log("Go to Scene 3 -> Check Build Settings"); // Remove when Level 2 works
                }
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