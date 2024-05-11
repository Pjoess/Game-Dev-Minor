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
        questLogText = "Try dodging and moving around \n\n" + $"-> Run along the path and use Dodge.";
        trigger.SetActive(false);
        TutorialEvents.OnEnterBuddy += Triggered;
    }

    public void Triggered(){
        isFinished = true;
    }
}
