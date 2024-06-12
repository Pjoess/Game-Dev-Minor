using UnityEngine;

public class DefeatBuddyStage : QuestStage
{

    [SerializeField] private GameObject boss;

    public override void StartStage()
    {
        boss.SetActive(true);
        Debug.Log("StartStage");
        isActive = true;
        questLogText = "Buddy has lost control. \n\n"
            + "-> Defeat Buddy";
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
