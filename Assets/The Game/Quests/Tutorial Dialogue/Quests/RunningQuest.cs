using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningQuest : QuestStage
{
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
        questLogText = "Try running and walking \n" + $"-> Run along the path";
        TutorialEvents.OnEnterCamera += Triggered;
    }

    public void Triggered(){
        isFinished = true;
    }
}
