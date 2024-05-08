using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraQuest : QuestStage
{
    public GameObject trigger;

    public override bool CheckStageCompleted()
    {
        if(isFinished){
            return true;
        }else{
            return false;
        }
    }

    public override void StartStage()
    {
        isActive = true;
        questLogText = "Try rotating camera and using zoom \n" + $"-> Run along the path and move camera";
        trigger.SetActive(false);
        TutorialEvents.OnEnterAttack += Triggered;
    }

    public void Triggered(){
        isFinished = true;
    }
}
