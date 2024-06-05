using UnityEngine;
using UnityEngine.SceneManagement;

public class ReachCastleStage : QuestStage
{
    [SerializeField] GameObject castleTrigger;

    public override void StartStage()
    {
        castleTrigger.SetActive(true);
        Debug.Log("StartStage");
        isActive = true;
        questLogText = "The Buddy has remembered how to open the castle gate. \n\n" 
            + "-> Go to the castle and confront the king.";
        QuestEvents.OnPlayerReachCastle += OnPlayerReachCastle;
    }

    public override bool CheckStageCompleted()
    {
        if (isFinished)
        {
            castleTrigger.SetActive(false);
            isActive = false;
            SceneManager.LoadScene(3);
            return true;
        }
        else return false;
    }

    private void OnPlayerReachCastle()
    {
        isFinished = true;
    }
}
