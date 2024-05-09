using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachCastleStage : QuestStage
{

    [SerializeField] GameObject castleTrigger;

    public override void StartStage()
    {
        castleTrigger.SetActive(true);
        Debug.Log("StartStage");
        isActive = true;
        questLogText = "The Buddy is acting strange and looks at the Castle, but why? \n\n" 
            + "-> Go to the Castle";
        QuestEvents.OnPlayerReachCastle += OnPlayerReachCastle;
    }

    public override bool CheckStageCompleted()
    {
        if (isFinished)
        {
            castleTrigger.SetActive(false);
            isActive = false;
            return true;
        }
        else return false;
    }

    private void OnPlayerReachCastle()
    {
        isFinished = true;
    }
}
