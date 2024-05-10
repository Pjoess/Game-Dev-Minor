using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackQuest : QuestStage
{
    public GameObject trigger;
    public GameObject nextTrigger;
    public static int slimesKilled = 0;

    public override bool CheckStageCompleted()
    {
        if(isFinished){
            return true;
        }else{
            return false;
        }
    }

    public override void StartStage()
    {
        isActive = true;
        questLogText = "Try doing an attack combo \n\n" + $"-> Kill slime dummies {slimesKilled}/2";
        trigger.SetActive(false);
        TutorialEvents.OnEnterAttack += Triggered;
    }

    public void Triggered(){
        isFinished = true;
    }
}
