using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerStunState : PlayerBaseState
{

    private float timer;

    public override void EnterState(Player_Manager player)
    {
        player.animator.SetBool(player.animIDHit, true);
        timer = player.stunTime;
    }

    public override void ExitState(Player_Manager player)
    {
        player.animator.SetBool(player.animIDHit, false);
    }

    public override void UpdateState(Player_Manager player)
    {
        if (timer > 0) timer -= Time.deltaTime;
        else player.ChangeState(player.idleState);
    }
}
