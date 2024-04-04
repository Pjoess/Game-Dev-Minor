using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyBase enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        enemy.Agent.isStopped = false;
        Debug.Log("Chase state");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if(enemy.CheckIdle())
        {
            enemyStateMachine.ChangeState(enemy.enemyIdleState);
        }

        if(enemy.CheckAttack())
        {
            enemyStateMachine.ChangeState(enemy.enemyAttackState);
        }

        enemy.Chase();
        // enemy.CheckChase();
    }
}