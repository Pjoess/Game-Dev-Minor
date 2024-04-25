using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{

    float elapsed = 0f;
    float duration = 0.5f;

    public override void EnterState(Player player)
    {
        //Debug.Log("Dodge");
        player.rollSound.Play();
        player.animator.SetBool(player.animIDDash, true);
        elapsed = 0f;
    }

    public override void ExitState(Player player)
    {
        player.isDashing = false;
        player.dashCooldownDelta = player.dashCooldown;
        player.animator.SetBool(player.animIDDash, false);
    }

    public override void UpdateState(Player player)
    {

        if (elapsed < duration)
        {
            player.transform.Translate(player.dashForce * Time.deltaTime * player.dashDirection, Space.World);
            elapsed += Time.deltaTime;
        }
        else
        {
            if (player.isSprinting) player.ChangeState(player.runState);
            else player.ChangeState(player.idleState);
        }
    }
}
