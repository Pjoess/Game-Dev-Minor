using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(NewEnemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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
        enemy.Agent.SetDestination(enemy.Target.position);

        if(enemy.IsWithinStrikingDistance)
        {
            enemyStateMachine.ChangeState(enemy.EnemyAttackState);
        }
        if(!enemy.IsAggroed)
        {
            enemyStateMachine.ChangeState(enemy.EnemyIdleState);
        }
    }
}