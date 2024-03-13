using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrike2State : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.sword.EnableSwordCollider();
        player.HasAttacked += player.OnAttackStruck;
        player.struckAgain = false;
        Debug.Log("Anim2");
    }

    public override void ExitState(Player player)
    {
        player.HasAttacked -= player.OnAttackStruck;
        player.sword.DisableSwordCollider();
    }

    public override void UpdateState(Player player)
    {
        player.AttackRotation();
        if (player.IsAnimFinished("Strike2"))
        {
            if (player.struckAgain)
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
}
