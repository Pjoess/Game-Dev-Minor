using UnityEngine;

public class PlayerHitState : PlayerBaseState
{

    private float strikeTimer = 1f;
    private float strikeTimerDelta = 1f;

    public override void EnterState(Player player)
    {
        player.animator.SetBool(player.animIDStriking, true);
        strikeTimerDelta = strikeTimer;
    }

    public override void ExitState(Player player)
    {
        player.animator.SetBool(player.animIDStriking, false);
    }

    public override void UpdateState(Player player)
    {
        if (strikeTimerDelta > 0)
        {
            strikeTimerDelta -= Time.deltaTime;
        }
        else
        {
            player.ChangeState(player.idleState);
        }
    }
}