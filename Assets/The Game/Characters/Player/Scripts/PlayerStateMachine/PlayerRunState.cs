using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public override void EnterState(Player_Manager player)
    {
        // player.footStepSound.pitch = 1.25f;
        // player.footStepSound.Play();
    }

    public override void ExitState(Player_Manager player)
    {
        // player.footStepSound.pitch = 1.0f;
        // player.footStepSound.Stop();
    }

    public override void UpdateState(Player_Manager player)
    {
        player.Movement();
        player.Jump();
        player.Dash();
        player.Attack();
        if (player.movement == Vector2.zero) player.ChangeState(player.idleState);
        if (!player.isSprinting && !player.sprintToggle) player.ChangeState(player.walkState);
        player.FallCheck();
    }
}