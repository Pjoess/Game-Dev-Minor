using System;
using UnityEngine;

public class PlayerStrikeState : PlayerBaseState
{
    private bool struckAgain;

    public override void EnterState(Player player)
    {
        player.sword.EnableSwordCollider();
        Player.hasAttacked += OnAttack;
        player.animator.SetBool(player.animIDStrike1, true);
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

        player.attackRotation();

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