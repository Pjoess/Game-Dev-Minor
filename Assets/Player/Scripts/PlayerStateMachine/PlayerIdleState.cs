using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        
    }

    public override void ExitState(Player player)
    {
        
    }

    public override void UpdateState(Player player)
    {
        if (player.movement != Vector2.zero) player.ChangeState(player.walkState);
        if (!player.GroundCheck()) player.ChangeState(player.fallState);
    }

}
