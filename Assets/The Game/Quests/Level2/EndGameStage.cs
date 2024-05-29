using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameStage : QuestStage
{

    [SerializeField] VictoryScript script;

    public override bool CheckStageCompleted()
    {
        return false;
    }

    public override void StartStage()
    {
        script.EnableVictoryCanvas(true);
    }
}
