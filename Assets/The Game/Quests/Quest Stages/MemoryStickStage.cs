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
        questLogText = "Your Buddy needs Memory Chips. Large Slimes have eaten them, Find those Chips and get them back! \n\n" +
            $"-> {sticksCollected} / 3 Memory Chips";
    }
}
