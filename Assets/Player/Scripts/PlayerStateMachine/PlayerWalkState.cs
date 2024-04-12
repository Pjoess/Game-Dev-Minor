using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        // player.footStepSound.pitch = 0.9f;
        // player.footStepSound.Play();
    }

    public override void ExitState(Player player)
    {
        // player.footStepSound.pitch = 1f;
        // player.footStepSound.Stop();
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