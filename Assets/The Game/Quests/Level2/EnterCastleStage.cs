using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCastleStage : QuestStage
{
    [SerializeField] GameObject trigger;

    public override void StartStage()
    {
        trigger.SetActive(true);
        Debug.Log("StartStage");
        isActive = true;
        questLogText = "The Buddy is dead. I must keep going. \n\n"
            + "-> Enter The Castle";
        QuestEvents.OnPlayerEnterCastle += OnPlayerEnterCastle;
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

    private void OnPlayerEnterCastle()
    {
        isFinished = true;
    }
}
