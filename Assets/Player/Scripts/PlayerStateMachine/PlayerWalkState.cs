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
        if (player.isSprinting) player.ChangeState(player.runState);
        if (!player.GroundCheck()) player.ChangeState(player.fallState);
    }
}