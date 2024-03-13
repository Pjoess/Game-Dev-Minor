using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrike2State : PlayerBaseState
{

    private bool struckAgain;

    public override void EnterState(Player player)
    {
        player.sword.EnableSwordCollider();
        Player.hasAttacked += OnAttack;
        struckAgain = false;
        Debug.Log("Anim2");
    }

    public override void ExitState(Player player)
    {
        Player.hasAttacked -= OnAttack;
        player.sword.DisableSwordCollider();

    }

    public override void UpdateState(Player player)
    {
        player.attackRotation();
        if (player.IsAnimFinished("Strike2"))
        {
            if (struckAgain)
            {
                player.animator.SetBool(player.animIDStrike3, true);
                player.ChangeState(player.strike3State);
            }
            else
            {
                player.animator.SetBool(player.animIDStrike1, false);
                player.animator.SetBool(player.animIDStrike2, false);
                player.ChangeState(player.idleState);
            }
        }
    }

    private void OnAttack()
    {
        struckAgain = true;
    }
}
