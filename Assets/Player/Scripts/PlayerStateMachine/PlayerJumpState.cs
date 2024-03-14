using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.animator.SetBool(player.animIDJump, true);
    }

    public override void ExitState(Player player)
    {
        player.animator.SetBool(player.animIDJump, false);
    }

    public override void UpdateState(Player player)
    {
        player.Movement();
        if (player.IsAnimFinished("JumpStart"))
        {
            player.ChangeState(player.fallState);
        }
    }
}
