using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClearRoomStage : QuestStage
{

    [SerializeField] private List<GameObject> slimes = new List<GameObject>();

    public override void StartStage()
    {
        isActive = true;
        questLogText = "Slimes are blocking the way forward. \n\n"
            +  "-> Clear the room.";
    }

    public override bool CheckStageCompleted()
    {
        CheckSlimesDefeated();
        if (isFinished)
        {
            isActive = false;
            return true;
        }
        else return false;
    }

    private void CheckSlimesDefeated()
    {
        if (!slimes.Any(x => x != null))
        {
            isFinished = true;
        }
    }
}
