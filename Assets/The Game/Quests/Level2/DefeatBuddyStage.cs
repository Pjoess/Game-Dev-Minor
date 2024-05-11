using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatBuddyStage : QuestStage
{

    public override void StartStage()
    {
        Debug.Log("StartStage");
        isActive = true;
        questLogText = "The Buddy has lost control.? \n\n"
            + "-> Defeat The Buddy";
        QuestEvents.OnBuddyDeath += OnBuddyDeath;
    }

    public override bool CheckStageCompleted()
    {
        if (isFinished)
        {
            isActive = false;
            return true;
        }
        else return false;
    }

    private void OnBuddyDeath()
    {
        isFinished = true;
    }
}
