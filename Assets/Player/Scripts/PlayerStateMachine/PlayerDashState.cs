using System.Collections;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.Dash();
    }

    public override void ExitState(Player player)
    {
        
    }

    public override void UpdateState(Player player)
    {
        if (player.movement == Vector2.zero) player.ChangeState(player.idleState);
        if (player.isSprinting) {
            player.ChangeState(player.runState);
        } else {
            player.ChangeState(player.walkState);
        }
    }
}
