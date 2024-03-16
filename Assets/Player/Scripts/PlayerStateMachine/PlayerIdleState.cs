using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        Debug.Log("Idle");
    }

    public override void ExitState(Player player)
    {
        
    }

    public override void UpdateState(Player player)
    {
        if (player.movement != Vector2.zero) player.ChangeState(player.walkState);
        player.Jump();
        player.FallCheck();
        player.Dash();
        player.Attack();

        player.animationBlend = Mathf.Lerp(player.animationBlend, 0, Time.deltaTime * player.speedChangeRate);
        if (player.animationBlend < 0.01f) player.animationBlend = 0f;
        player.animator.SetFloat(player.animIDSpeed, player.animationBlend);
    }
}