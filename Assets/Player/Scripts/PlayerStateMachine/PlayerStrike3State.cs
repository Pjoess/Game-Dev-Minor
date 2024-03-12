using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrike3State : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        Debug.Log("Anim3");
    }

    public override void ExitState(Player player)
    {
        player.animator.SetBool(player.animIDStrike1, false);
        player.animator.SetBool(player.animIDStrike2, false);
        player.animator.SetBool(player.animIDStrike3, false);
    }

    public override void UpdateState(Player player)
    {
        if(player.IsAnimFinished("Strike3")) player.ChangeState(player.idleState);
    }
}
