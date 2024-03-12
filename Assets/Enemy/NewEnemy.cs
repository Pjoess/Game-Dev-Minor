using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemy : MonoBehaviour, ITriggerCheckable
{
    #region Movement variables
        public Rigidbody RB { get; set; }
        public bool IsAggroed { get; set; }
        public bool IsWithinStrikingDistance { get; set; }
    #endregion

    #region States
        public EnemyStateMachine EnemyStateMachine { get; set; }
        public EnemyChaseState EnemyChaseState { get; set; }
        public EnemyFallState EnemyFallState { get; set; }
        public EnemyHitState EnemyHitState { get; set; }
        public EnemyIdleState EnemyIdleState { get; set; }
    #endregion

    #region AI variables
        public NavMeshAgent Agent { get; set; }
        public Transform Target { get; set; }
    #endregion



    #region Standard methods for enemy
        public void Awake()
        {
            EnemyStateMachine = new EnemyStateMachine();

            EnemyChaseState = new EnemyChaseState(this, EnemyStateMachine);
            EnemyFallState = new EnemyFallState(this, EnemyStateMachine);
            EnemyHitState = new EnemyHitState(this, EnemyStateMachine);
            EnemyIdleState = new EnemyIdleState(this, EnemyStateMachine);

            Agent = GetComponent<NavMeshAgent>();
            Target = GameObject.FindWithTag("Player").transform;
        }
        void Start()
        {
            RB = GetComponent<Rigidbody>();

            EnemyStateMachine.Initialize(EnemyIdleState);
            Agent.speed = 1.5f;
        }
        void Update()
        {
            EnemyStateMachine.CurrentEnemyState.UpdateState();

        }
    #endregion

    #region Enemy Movement
        public void SetAggroStatus(bool isAggroed)
        {
            IsAggroed = isAggroed;
        }

        public void SetStrikingDistanceBool(bool isWithinStrikingDistance)
        {
            IsWithinStrikingDistance = isWithinStrikingDistance;
        }
    #endregion
}
