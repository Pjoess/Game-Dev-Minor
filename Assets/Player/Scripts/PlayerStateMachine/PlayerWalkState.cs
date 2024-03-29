using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
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
        player.Jump();
        player.Dash();
        player.Attack();
        if (player.movement == Vector2.zero) player.ChangeState(player.idleState);
        if (player.isSprinting) player.ChangeState(player.runState);
        player.FallCheck();
    }
}