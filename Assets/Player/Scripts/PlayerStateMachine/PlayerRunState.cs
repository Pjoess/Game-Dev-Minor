using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public override void EnterState(Player player)
    {

    }

    public override void ExitState(Player player)
    {

    }

    public override void UpdateState(Player player)
    {
        player.Movement(player.runSpeed);
        if (!player.isSprinting) player.ChangeState(player.walkState);
        if (!player.GroundCheck()) player.ChangeState(player.fallState);
    }
}
