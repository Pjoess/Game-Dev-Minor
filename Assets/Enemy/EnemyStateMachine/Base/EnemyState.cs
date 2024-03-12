using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyState
{
    protected NewEnemy enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(NewEnemy enemy, EnemyStateMachine enemyStateMachine)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }
    public virtual void EnterState() {}
    public virtual void ExitState() {}
    public virtual void UpdateState() {}
}