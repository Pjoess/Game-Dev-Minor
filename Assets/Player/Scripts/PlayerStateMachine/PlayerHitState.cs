using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private readonly float strikeTimer = 1f;
    private float strikeTimerDelta;

    public override void EnterState(Player player)
    {
        player.animator.SetBool(player.animIDStriking, true);
        strikeTimerDelta = strikeTimer;
    }

    public override void ExitState(Player player)
    {
        player.sword.DisableSwordCollider();
        player.animator.SetBool(player.animIDStriking, false);
    }

    public override void UpdateState(Player player)
    {   
        if (strikeTimerDelta > 0) {
            player.sword.EnableSwordCollider();
            strikeTimerDelta -= Time.deltaTime;
        } else {
            player.ChangeState(player.idleState);
        }
    }
}