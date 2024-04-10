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
        questLogText = "The buddy detects another memory stick inside the castle. \n" +
            "Go to the castle";
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
