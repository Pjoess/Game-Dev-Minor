using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(Player_Manager player)
    {
        player.animator.SetBool(player.animIDJump, true);
        player.jumpCooldownDelta = player.jumpCooldown;
        player.rigidBody.AddForce(Vector3.up * player.jumpForce, ForceMode.Impulse);
        //player.jumpSound.Play();
    }

    public override void ExitState(Player_Manager player)
    {
        player.animator.SetBool(player.animIDJump, false);
    }

    public override void UpdateState(Player_Manager player)
    {
        player.Movement();
        if (player.IsAnimFinished("JumpStart"))
        {
            player.ChangeState(player.fallState);
        }
    }
}
