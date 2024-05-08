using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeQuest : QuestStage
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
        questLogText = "Try dodging and moving around with it \n" + $"-> Run along the path using dodge";
        trigger.SetActive(false);
        TutorialEvents.OnEnterDodge += Triggered;
    }

    public void Triggered(){
        isFinished = true;
    }
}
