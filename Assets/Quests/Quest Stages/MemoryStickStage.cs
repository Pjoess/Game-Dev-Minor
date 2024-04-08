using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryStickStage : QuestStage
{
    private int sticksCollected = 0;
    public override void StartStage()
    {
        Debug.Log("StartStage");
        isActive = true;
        UpdateText();
        QuestEvents.OnMemoryStickPickUp += StickPickUp;
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

    private void StickPickUp()
    {
        sticksCollected++;
        UpdateText();
        if(sticksCollected == 3)
        {
            isFinished = true;
        }
    }

    private void UpdateText()
    {
        questLogText = "Your buddy needs memory sticks. Find the large slimes inside the village and slay them to aquire memory sticks \n" +
            $"Memory stick collected: {sticksCollected} / 3";
    }
}
