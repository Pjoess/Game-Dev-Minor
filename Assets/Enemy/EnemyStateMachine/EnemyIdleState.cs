using UnityEngine;
public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(NewEnemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        enemy.Agent.isStopped = true;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (enemy.IsAggroed)
        {
            enemyStateMachine.ChangeState(enemy.EnemyChaseState);
        }
    }
}