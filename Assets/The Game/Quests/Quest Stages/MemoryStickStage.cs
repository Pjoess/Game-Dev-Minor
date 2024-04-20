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
        questLogText = "Your Buddy needs Memory Sticks. Large Slimes have eaten them, Find those Sticks and get them back! \n\n" +
            $"-> Memory Sticks: {sticksCollected} / 3 obtained.";
    }
}
