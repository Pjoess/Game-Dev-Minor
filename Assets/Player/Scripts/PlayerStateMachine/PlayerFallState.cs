using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        Debug.Log("Fall");
    }

    public override void ExitState(Player player)
    {
        Debug.Log("Land");
    }

    public override void UpdateState(Player player)
    {
        if (player.GroundCheck()) player.ChangeState(player.idleState);
    }
}
