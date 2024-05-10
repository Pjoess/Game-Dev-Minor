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
        questLogText = "Try using the buddy attacks \n\n" + $"-> Run along the path and kill the slimes. When you are done, enter the portal.";
        trigger.SetActive(false);
        TutorialEvents.OnEnterBuddy += Triggered;
    }

    public void Triggered(){
        isFinished = true;
    }
}
