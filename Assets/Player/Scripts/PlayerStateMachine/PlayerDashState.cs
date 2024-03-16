using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{

    float elapsed = 0f;
    float duration = 0.4f;

    public override void EnterState(Player player)
    {
        elapsed = 0f;
    }

    public override void ExitState(Player player)
    {
        player.isDashing = false;
        player.dashCooldownDelta = player.dashCooldown;
    }

    public override void UpdateState(Player player)
    {
        player.transform.Translate(player.dashForce * Time.deltaTime * player.dashDirection, Space.World);
        if (elapsed < duration)
        {
            elapsed += Time.deltaTime;
        }
        else
        {
            player.ChangeState(player.idleState);
        }
    }
}
