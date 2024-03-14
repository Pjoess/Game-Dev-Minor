using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        Debug.Log("Fall");
        player.animator.SetBool(player.animIDGrounded, false);
    }

    public override void ExitState(Player player)
    {
        Debug.Log("Land");
        player.animator.SetBool(player.animIDGrounded, true);
        player.animator.SetBool(player.animIDJump, false);
        player.animator.SetBool(player.animIDFall, false);
    }

    public override void UpdateState(Player player)
    { 
        player.Movement();
        if (player.IsGrounded())
        {
            player.jumpToFallDelta = player.jumpToFallTimer;
            player.ChangeState(player.idleState);   
        }
    }
}