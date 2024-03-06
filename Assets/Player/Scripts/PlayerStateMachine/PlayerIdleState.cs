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
        if (!player.GroundCheck()) player.ChangeState(player.fallState);

        player._animationBlend = Mathf.Lerp(player._animationBlend, 0, Time.deltaTime * 5f);
        if (player._animationBlend < 0.01f) player._animationBlend = 0f;

        player.animator.SetFloat(player.animIDSpeed, player._animationBlend);
    }

}
