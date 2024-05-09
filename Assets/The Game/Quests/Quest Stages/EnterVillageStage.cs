using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterVillageStage : QuestStage
{

    [SerializeField] GameObject villageTrigger;

    public override void StartStage()
    {
        villageTrigger.SetActive(true);
        Debug.Log("StartStage");
        isActive = true;
        questLogText = "Something feels wrong? \n\n" + $"-> Enter the Village.";
        QuestEvents.OnPlayerEnterVillage += EnteredVillage;
    }

    public override bool CheckStageCompleted()
    {
        if (isFinished)
        {
            villageTrigger.SetActive(false);
            isActive = false;
            return true;
        }
        else return false;
    }
    
    private void EnteredVillage()
    {
        isFinished = true;
    }
}
