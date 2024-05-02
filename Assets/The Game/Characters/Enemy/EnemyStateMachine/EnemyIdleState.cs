using UnityEngine;
public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyBase enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        // if(enemy.Agent == null){
        //     Debug.Log("enemy is null");
        // }
        // enemy.Agent.isStopped = true;
        enemy.Idle();
    }

    public override void ExitState()
    {
        enemy.ExitIdle();
    }

    public override void UpdateState()
    {
        if(enemy.CheckChase())
        {
            enemyStateMachine.ChangeState(enemy.enemyChaseState);
        }

        if(enemy.CheckAttack())
        {
            enemyStateMachine.ChangeState(enemy.enemyAttackState);
        }
    }
}