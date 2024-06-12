using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfrontTheKing : QuestStage
{
    public override void StartStage()
    {
       isActive = true;
       questLogText = "The way is clear, it's time to confront the King\n\n"
            + "-> Go to the throne room.";
       QuestEvents.OnPlayerEnterThroneRoom += EnterThroneRoom;
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

    private void EnterThroneRoom()
    {
        isFinished = true;
    }

}
