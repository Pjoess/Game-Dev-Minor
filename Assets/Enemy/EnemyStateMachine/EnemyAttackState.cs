using UnityEngine;
public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyBase enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        enemy.Agent.isStopped = true;
        Debug.Log("Attacking...");
        enemy.Attack();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if(enemy.CheckChase() && !enemy.IsAttacking)
        {
            enemyStateMachine.ChangeState(enemy.enemyChaseState);
        }
        if(enemy.CheckIdle() && !enemy.IsAttacking)
        {
            enemyStateMachine.ChangeState(enemy.enemyIdleState);
        }
        if(enemy.CheckAttack() && !enemy.IsAttacking)
        {
            EnterState();
        }
    }
}