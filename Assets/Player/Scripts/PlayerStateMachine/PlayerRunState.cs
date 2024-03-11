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
        player.Movement();
        if (player.movement == Vector2.zero) player.ChangeState(player.idleState);
        if (!player.isSprinting) player.ChangeState(player.walkState);
        player.FallCheck();
    }
}