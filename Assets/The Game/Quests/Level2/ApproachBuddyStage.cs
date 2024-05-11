using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachBuddyStage : QuestStage
{
    [SerializeField] GameObject trigger;

    public override void StartStage()
    {
        trigger.SetActive(true);
        Debug.Log("StartStage");
        isActive = true;
        questLogText = "The Buddy is acting strange, but why? \n\n"
            + "-> Go to the Buddy";
        QuestEvents.OnPlayerReachBuddy += OnPlayerReachBuddy;
    }

    public override bool CheckStageCompleted()
    {
        if (isFinished)
        {
            trigger.SetActive(false);
            isActive = false;
            return true;
        }
        else return false;
    }

    private void OnPlayerReachBuddy()
    {
        isFinished = true;
    }
}
