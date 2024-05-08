using UnityEngine;

public class EnemyFallState : EnemyState
{
    public EnemyFallState(EnemyBase enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    //private float fallTimer;

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    { 
        enemy.CheckFall();
        
    }
}