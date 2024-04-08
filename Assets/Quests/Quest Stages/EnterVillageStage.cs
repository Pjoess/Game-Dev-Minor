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
        questLogText = "Enter the village";
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
