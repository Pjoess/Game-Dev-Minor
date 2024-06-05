using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackQuest : QuestStage
{
    public GameObject trigger;
    public Collider nextTrigger;
    public int slimesKilled;
    public bool WalkedOverTrigger;
    public bool DidCombo;

    public bool isAttacking;
    public float timer;
    public int attackStack;

    void Awake(){
        slimesKilled = 0;
        slimesKilled = default;
        timer = 0;
        attackStack = 0;
        isAttacking = false;
    }

    public override bool CheckStageCompleted()
    {
        // if(!DidCombo){
        //     timer += Time.deltaTime;
        // }
        // Debug.Log(timer);
        // if(isAttacking && attackStack >= 3){
        //     DidCombo = true;
        //     Debug.Log("Finished attack combo");
        // }
        // if(Input.GetMouseButtonDown(0) && !DidCombo){

        //     if(isAttacking && timer < 0.15f){
        //         attackStack++;
        //     }else{
        //         isAttacking = true;
        //         timer = 0;
        //         attackStack = 0;
        //     }
        // }
        if(isFinished && WalkedOverTrigger ){ // && DidCombo
            return true;
        }else{
            return false;
        }
    }

    public override void StartStage()
    {
        slimesKilled = 0;
        WalkedOverTrigger = false;
        DidCombo = false;
        isActive = true;
        questLogText = "Try doing an attack combo \n\n" + $"-> Kill slime dummies {slimesKilled}/4 \n" + $"";
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
