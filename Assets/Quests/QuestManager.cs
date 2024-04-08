using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private bool questCompleted = false;
    [SerializeField] private TMP_Text questLog;

    [SerializeField] QuestStage[] questStages;
    private int currentStage = 0;

    // Start is called before the first frame update
    void Start()
    {
        questStages[currentStage].StartStage();
        UpdateLog(questStages[currentStage].questLogText);
    }

    // Update is called once per frame
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