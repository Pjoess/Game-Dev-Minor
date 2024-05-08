using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(Player_Manager player)
    {
        //Debug.Log("Idle");
        //player.isSprinting = false;
    }

    public override void ExitState(Player_Manager player)
    {
        
    }

    public override void UpdateState(Player_Manager player)
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