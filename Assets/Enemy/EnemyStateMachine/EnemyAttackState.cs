using UnityEngine;
public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyBase enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        enemy.Agent.isStopped = true;
        enemy.Attack();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if(enemy.CheckChase())
        {
            enemyStateMachine.ChangeState(enemy.enemyChaseState);
        }
        if(enemy.CheckIdle())
        {
            enemyStateMachine.ChangeState(enemy.enemyIdleState);
        }
    }
}