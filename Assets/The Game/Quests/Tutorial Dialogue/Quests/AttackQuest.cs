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
            // nextTrigger.GetComponent<BoxCollider>().isTrigger = true;
            return true;
        }else{
            return false;
        }
    }

    public override void StartStage()
    {
        isActive = true;
        questLogText = "Try out attacking and using the combo \n" + $"-> Kill slime dummy's {slimesKilled}/2";
        trigger.SetActive(false);
        // nextTrigger.GetComponent<BoxCollider>().isTrigger = false;
        TutorialEvents.OnEnterAttack += Triggered;
    }

    public void Triggered(){
        // if(slimesKilled >= 2){
        isFinished = true;
        // }
    }
}
