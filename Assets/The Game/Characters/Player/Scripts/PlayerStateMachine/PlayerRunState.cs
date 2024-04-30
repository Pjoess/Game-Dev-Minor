using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        // player.footStepSound.pitch = 1.25f;
        // player.footStepSound.Play();
    }

    public override void ExitState(Player player)
    {
        // player.footStepSound.pitch = 1.0f;
        // player.footStepSound.Stop();
    }

    public override void UpdateState(Player player)
    {
        player.Movement();
        player.Jump();
        player.Dash();
        player.Attack();
        if (player.movement == Vector2.zero) player.ChangeState(player.idleState);
        if (!player.isSprinting) player.ChangeState(player.walkState);
        player.FallCheck();
    }
}