using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackQuest : QuestStage
{
    public GameObject trigger;
    public Collider nextTrigger;
    public static int slimesKilled = 0;
    public bool WalkedOverTrigger;

    void Awake(){
        slimesKilled = 0;
    }

    public override bool CheckStageCompleted()
    {
        if(isFinished && WalkedOverTrigger){
            return true;
        }else{
            return false;
        }
    }

    public override void StartStage()
    {
        WalkedOverTrigger = false;
        isActive = true;
        questLogText = "Try doing an attack combo \n\n" + $"-> Kill slime dummies {slimesKilled}/4";
        trigger.SetActive(false);
        nextTrigger.isTrigger = false;
        TutorialEvents.OnEnterAttack += Triggered;
        TutorialEvents.OnTriggerDodge += TriggerDodge;
    }
    
    public void Triggered(){
        if(slimesKilled < 3){
            slimesKilled++;
            questLogText = "Try doing an attack combo \n\n" + $"-> Kill slime dummies {slimesKilled}/4";
        }else if(slimesKilled == 3){
            slimesKilled++;
            // nextTrigger.gameObject.SetActive(true);
            nextTrigger.isTrigger = true;
            questLogText = "Try doing an attack combo \n\n" + $"-> Kill slime dummies {slimesKilled}/4";
            slimesKilled = 0;
            isFinished = true;
            
        }
        
    }

    public void TriggerDodge(){
        WalkedOverTrigger = true;
    }
}
