using System;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private readonly float strikeTimer = 1f;
    private float strikeTimerDelta;
    private string currentAnim;
    private string nextAnim;
    private bool canClick;

    public override void EnterState(Player player)
    {
        player.animator.SetBool(player.animIDStrike1, true);
        strikeTimerDelta = strikeTimer;
        currentAnim = "Strike1";
        nextAnim = "Idle";
        canClick = true;
        Debug.Log("Anim1");
    }

    public override void ExitState(Player player)
    {
        player.sword.DisableSwordCollider();
        player.animator.SetBool(player.animIDStrike1, false);
        player.animator.SetBool(player.animIDStrike2, false);
        player.animator.SetBool(player.animIDStrike3, false);
    }

    public override void UpdateState(Player player)
    {
        //if (strikeTimerDelta > 0) {
        //    player.sword.EnableSwordCollider();
        //    strikeTimerDelta -= Time.deltaTime;
        //} else {
        //    //player.ChangeState(player.idleState);
        //}

        if(player.IsAnimFinished(currentAnim))
        {
            switch (nextAnim)
            {
                case "Idle":
                    player.ChangeState(player.idleState);
                    break;

                case "Strike2":
                    currentAnim = "Strike2";
                    player.animator.SetBool(player.animIDStrike2, true);
                    Debug.Log("Anim2");
                    nextAnim = "Idle";
                    canClick = true;
                    break;

                case "Strike3":
                    currentAnim = "Strike3";
                    player.animator.SetBool(player.animIDStrike3, true);
                    Debug.Log("Anim3");
                    nextAnim = "Idle";
                    break;
            }
        }
    }

    public void RespondToAttack(Player player)
    {
        if(canClick) 
        {
            switch (currentAnim)
            {
                case "Strike1":
                    nextAnim = "Strike2";
                    canClick = false;
                    break;

                case "Strike2":
                    nextAnim = "Strike3";
                    canClick = false;
                    break;
            }
        }
    }
}