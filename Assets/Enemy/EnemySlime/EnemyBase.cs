using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour, IChaseTriggerCheckable, IAttackDistanceTriggerCheckable
{
    #region Variables
        //
        public float HealthPoints { get; set; }
        public float MaxHealth { get; set; }
        public float MovementSpeed { get; set; }

        //Target nav
        public NavMeshAgent Agent { get; set; }
        public Transform Target { get; set; }

        //Triggers
        public bool IsAggroed { get; set; }
        public bool IsWithinStrikingDistance { get; set; }
        public bool IsAttacking { get; set; }

        [SerializeField]public EnemyHealthBar enemyHealthBar;
        public bool isCollisionCooldown;
        public float collisionCooldown = 1f;
    #endregion

    #region States
        public EnemyStateMachine enemyStateMachine { get; set; }
        public EnemyChaseState enemyChaseState { get; set; }
        public EnemyFallState enemyFallState { get; set; }
        public EnemyHitState enemyHitState { get; set; }
        public EnemyIdleState enemyIdleState { get; set; }
        public EnemyAttackState enemyAttackState { get; set; }
    #endregion

    #region Abstract Functions
        public abstract void Chase();
        public abstract void Attack();
        public abstract void Idle();
        public abstract void Hit(float damage);

        public abstract void InitializeStates();
        public abstract void Awake();
    #endregion

    #region State Functions
        public abstract bool CheckAttack();
        public abstract bool CheckChase();
        public abstract bool CheckFall();
        public abstract bool CheckHit();
        public abstract bool CheckIdle();
        // public bool CheckAttack()
        // {
        //     if(IsWithinStrikingDistance)
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }

        // public bool CheckChase()
        // {
        //     if(IsAggroed && !IsWithinStrikingDistance)
        //     {
        //         return true;
        //     }
        //     else 
        //     {
        //         return false;
        //     }
        // }

        // public bool CheckFall()
        // {
        //     throw new System.NotImplementedException();
        // }

        // public bool CheckHit()
        // {
        //     throw new System.NotImplementedException();
        // }

        // public bool CheckIdle()
        // {
        //     if (!IsAggroed && !IsWithinStrikingDistance)
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }
    #endregion



    public void Update()
    {
        enemyStateMachine.CurrentEnemyState.UpdateState();
    }

    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }
    public void SetStrikingDistanceBool(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
    }
    public void SetAgentDestination()
    {
        Agent.SetDestination(Target.position);
    }
}
