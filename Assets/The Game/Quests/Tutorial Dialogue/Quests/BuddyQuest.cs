using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyQuest : QuestStage
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
        questLogText = "Try out the buddy attacks \n" + $"-> Run along the path and kill the slimes. Then enter the portal";
        trigger.SetActive(false);
        TutorialEvents.OnEnterBuddy += Triggered;
    }

    public void Triggered(){
        isFinished = true;
    }
}
