using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public override void EnterState(Player_Manager player)
    {
        Debug.Log("Fall");
        player.animator.SetBool(player.animIDGrounded, false);
    }

    public override void ExitState(Player_Manager player)
    {
        Debug.Log("Land");
        player.animator.SetBool(player.animIDGrounded, true);
        player.animator.SetBool(player.animIDFall, false);
        player.hasJumped = false;
    }

    public override void UpdateState(Player_Manager player)
    { 
        player.Movement();
        if (player.IsGrounded())
        {
            player.idleToFallDelta = player.idleToFallTimer;
            player.ChangeState(player.idleState);   
        }
    }
}