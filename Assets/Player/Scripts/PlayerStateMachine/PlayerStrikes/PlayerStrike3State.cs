using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrike3State : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.sword.EnableSwordCollider();
        Debug.Log("Anim3");
        player.sword.GetComponent<MeshRenderer>().material.color = new Color32(204, 0, 0, 200); // Red
    }

    public override void ExitState(Player player)
    {
        player.sword.DisableSwordCollider();
        player.animator.SetBool(player.animIDStrike1, false);
        player.animator.SetBool(player.animIDStrike2, false);
        player.animator.SetBool(player.animIDStrike3, false);
        player.sword.SwordToDefault();
    }

    public override void UpdateState(Player player)
    {
        player.AttackRotation();
        if (player.IsAnimFinished("Strike3")) player.ChangeState(player.idleState);
    }
}
