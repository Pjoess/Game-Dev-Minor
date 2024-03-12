using System;
using UnityEngine;

public class PlayerStrikeState : PlayerBaseState
{
    private readonly float strikeTimer = 1f;
    private float strikeTimerDelta;

    private bool struckAgain;

    public override void EnterState(Player player)
    {
        Player.hasAttacked += OnAttack;
        player.animator.SetBool(player.animIDStrike1, true);
        strikeTimerDelta = strikeTimer;
        struckAgain = false;
        Debug.Log("Anim1");
    }

    public override void ExitState(Player player)
    {
        Player.hasAttacked -= OnAttack;
        player.sword.DisableSwordCollider();
    }

    public override void UpdateState(Player player)
    {
        //if (strikeTimerDelta > 0) {
        //    player.sword.EnableSwordCollider();
        //    strikeTimerDelta -= Time.deltaTime;
        //} else {
        //    //player.ChangeState(player.idleState);
        //}

        if(player.IsAnimFinished("Strike1"))
        {
            if(struckAgain) 
            {
                player.animator.SetBool(player.animIDStrike2, true);
                player.ChangeState(player.strike2State);
            }
            else
            {
                player.animator.SetBool(player.animIDStrike1, false);
                player.ChangeState(player.idleState);
            }
        }
    }

    private void OnAttack()
    {
        struckAgain = true;
    }
}