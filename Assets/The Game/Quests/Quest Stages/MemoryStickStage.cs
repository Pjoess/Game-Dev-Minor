using UnityEngine;

public class MemoryStickStage : QuestStage
{
    private int sticksCollected = 0;
    [SerializeField] private Dialogue[] dialogues;
    public override void StartStage()
    {
        sticksCollected = 0;
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
        dialogues[sticksCollected].TriggerDialgue();
        sticksCollected++;
        UpdateText();
        if(sticksCollected == 3)
        {
            isFinished = true;
        }
    }

    private void UpdateText()
    {
        if(sticksCollected > 0)
        {
            questLogText = $"{sticksCollected} / 3 Memory Chips Collected";
        }
        else
        {
            questLogText = "Your Buddy needs Memory Chips. Large Slimes have eaten them, Find those Chips and get them back! \n\n" +
            "--- Task --- \n" +
            $"{sticksCollected} / 3 Memory Chips";
        }
    }
}
