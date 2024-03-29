using UnityEngine;
public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(NewEnemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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
        //rotate towards player

        //Attack player
        if(enemy.IsAttacking == null)
        {
            enemy.SetIsAttackingBool(true);
            enemy.Attack();
        }
        
        if(!enemy.IsWithinStrikingDistance && enemy.IsAggroed)
        {
            enemy.Agent.isStopped = false;
            enemyStateMachine.ChangeState(enemy.EnemyChaseState);
        }
    }
}