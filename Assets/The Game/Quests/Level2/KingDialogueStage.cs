using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    }

    public override bool CheckStageCompleted()
    {
        CheckDialogueOver();
        if (isFinished)
        {
            isActive = false;
            blackScreen.DisableBlackScreen();
            return true;
        }
        else return false;
    }

    private void CheckDialogueOver()
    {
        if (manager.isActive) isFinished = true;
    }
}
