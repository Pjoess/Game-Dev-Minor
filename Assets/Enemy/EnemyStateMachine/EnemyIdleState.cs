using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
    }

    public override void ExitState(Enemy enemy)
    {
        
    }

    public override void UpdateState(Enemy enemy)
    {
        if (enemy.IsAggroed)
        {
            // enemy.ChangeState(enemy.chaseState);
        }
    }
}