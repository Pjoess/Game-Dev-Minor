using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KingDialogueStage : QuestStage
{
    [SerializeField] private BlackScreenScript blackScreen;
    private DialogueManager manager;

    public override void StartStage()
    {
        isActive = true;
        blackScreen.EnableBlackScreen();
        GetComponent<Dialogue>().TriggerDialgue();
        manager = FindObjectOfType<DialogueManager>();
        Blackboard.instance.DisablePlayerInput();
    }

    public override bool CheckStageCompleted()
    {
        if (isFinished)
        {
            isActive = false;
            Blackboard.instance.EnablePlayerInput();
            SceneManager.LoadScene(4);
            return true;
        }
        else
        {
            CheckDialogueOver();
            return false;
        }
    }

    private void CheckDialogueOver()
    {
        if (!manager.isActive) isFinished = true;
    }
}
