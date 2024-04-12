// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class NewEnemy : MonoBehaviour, ITriggerCheckable, IDamageble
// {
//     #region General variables
//         public int HealthPoints { get; set; }
//         public int MaxHealth { get; set; } = 3;
//         public float MoveSpeed { get; set; }
        
//     #endregion

//     #region Movement variables
//         public Rigidbody RB { get; set; }
//         public bool IsAggroed { get; set; }
//         public bool IsWithinStrikingDistance { get; set; }
//         public IEnumerator IsAttacking { get; set; }
//     #endregion

//     #region States
//         public EnemyStateMachine EnemyStateMachine { get; set; }
//         public EnemyChaseState EnemyChaseState { get; set; }
//         public EnemyFallState EnemyFallState { get; set; }
//         public EnemyHitState EnemyHitState { get; set; }
//         public EnemyIdleState EnemyIdleState { get; set; }
//         public EnemyAttackState EnemyAttackState { get; set; }
//     #endregion

//     #region AI variables
//         public NavMeshAgent Agent { get; set; }
//         public Transform Target { get; set; }
//     #endregion



//     #region Standard methods for enemy
//         public void Awake()
//         {
//             EnemyStateMachine = new EnemyStateMachine();

//             EnemyChaseState = new EnemyChaseState(this, EnemyStateMachine);
//             EnemyFallState = new EnemyFallState(this, EnemyStateMachine);
//             EnemyHitState = new EnemyHitState(this, EnemyStateMachine);
//             EnemyIdleState = new EnemyIdleState(this, EnemyStateMachine);
//             EnemyAttackState = new EnemyAttackState(this, EnemyStateMachine);

//             Agent = GetComponent<NavMeshAgent>();
//             Target = GameObject.FindWithTag("Player").transform;
//         }
//         void Start()
//         {
//             RB = GetComponent<Rigidbody>();

//             EnemyStateMachine.Initialize(EnemyIdleState);
//             Agent.speed = 1.5f;
//             HealthPoints = MaxHealth;
//         }
//         void Update()
//         {
//             EnemyStateMachine.CurrentEnemyState.UpdateState();

//         }
//     #endregion

//     #region Enemy States functions
//         public void SetAggroStatus(bool isAggroed)
//         {
//             IsAggroed = isAggroed;
//         }

//         public void SetStrikingDistanceBool(bool isWithinStrikingDistance)
//         {
//             IsWithinStrikingDistance = isWithinStrikingDistance;
//         }

//         public void SetIsAttackingBool(bool isAttacking)
//         {
//             // IsAttacking = isAttacking;
//         }

//         public void FacePlayer()
//         {
//             Vector3 directionToPlayer = (Target.position - transform.position).normalized;
//             Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
//             transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 100);
//         }

//         public void Attack()
//         {
//             FacePlayer();
//             IsAttacking = EnemyAttackCoroutine();
//             StartCoroutine(IsAttacking);
//             IsAttacking = null;
//         }
//     #endregion

//     #region Coroutines
//         IEnumerator EnemyAttackCoroutine()
//         {
//             GetComponent<MeshRenderer>().material.color = new Color32(170, 0, 0, 200);
//             yield return new WaitForSeconds(1);
//             GetComponent<MeshRenderer>().material.color = new Color32(0, 0, 0, 200);
//         }
//     #endregion

//     #region Attacked 

//         public void Hit()
//         {
            
//         }
//     #endregion
// }
