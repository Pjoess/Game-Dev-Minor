using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BasicEnemySlime
{
    public class BasicEnemySlime : MonoBehaviour, IDamageble
    {
        private IBaseNode basicSlimeBT = null;
        public LayerMask attackLayer; // Player
        private NavMeshAgent agent;
        private Rigidbody rigidBody;
        public static float originalSpeed;

        [Header("Patrol Center Point")]
        public GameObject patrolCenterPoint;

        [Header("Patrol Settings")]
        private float patrolRadius = 20f;
        private float stopDistance = 1f;

        [Header("Chase")]
        private float chaseRange = 10f;

        [Header("Attack")]
        private float attackRange = 8f;
        private float offsetDistance = 1f;
        public static bool hasAttacked = false;
        
        // --- IDamagable --- //
        [Header("Stats")]
        private EnemyHealthBar enemyHealthBar;
        public int healthPoints;
        public int maxHealthPoints = 30;
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
    
        [Header("Cone Settings")]
        private float coneWidth = 50f;
        private float coneLength = 3f;
        private float thickness = 2f;

        private Animator animator;
        private int animIDAnticipate;
        private int animIDAttack;
        private int animIDWalking;

        private void AssignAnimIDs()
        {
            animIDAnticipate = Animator.StringToHash("isAnticipating");
            animIDAttack = Animator.StringToHash("isAttacking");
            animIDWalking = Animator.StringToHash("isWalking");
        }

        private void Awake()
        {
            AssignAnimIDs();
            HealthPoints = MaxHealthPoints;
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
            rigidBody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            originalSpeed = agent.speed;
            BehaviourTree();
        }

        void Update()
        {
            basicSlimeBT?.Update(); // Update the boss behavior tree
        }

        #region Behaviour Tree
        private void BehaviourTree()
        {

            List<IBaseNode> IsPlayerInLineOfSight = new()
            {
                new ChasePlayerNode(agent,chaseRange,stopDistance,animator,animIDWalking),
                new AttackPlayerNode(agent,attackRange,stopDistance,attackLayer,coneWidth,coneLength,animator,animIDAnticipate,animIDAttack),
            };

            List<IBaseNode> IsPlayerNotInLineOfSight = new()
            {
                new PatrolNode(agent, patrolCenterPoint, patrolRadius, stopDistance, chaseRange, attackRange,animator,animIDWalking),
            };

            List<IBaseNode> Root = new()
            {
                new SequenceNode(IsPlayerInLineOfSight),
                new SequenceNode(IsPlayerNotInLineOfSight),
            };

            basicSlimeBT = new SelectorNode(Root);
        }
        #endregion

        #region Cone Raycast
        private void OnDrawGizmos()
        {
            // Draw cone shape in Gizmos
            DrawCone(transform.position, transform.forward, coneWidth, coneLength, thickness);

            // Draw chase range sphere
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);

            if(patrolCenterPoint != null){
                // Draw patrol radius
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(patrolCenterPoint.transform.position, patrolRadius);
            }

            // Draw stop distance
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, stopDistance);
        }

        // Draw cone shape in Gizmos
        private void DrawCone(Vector3 origin, Vector3 direction, float coneWidth, float coneLength, float thickness)
        {
            // Store the current Gizmos color
            Color previousColor = Gizmos.color;

            // Set the desired color
            Gizmos.color = Color.red;

            // Calculate half width
            float halfWidth = coneWidth / 2f;

            // Calculate offset for thickness
            Vector3 offset = Vector3.up * thickness;

            // Draw cone base with thickness
            Gizmos.DrawLine(origin - offset, origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength - offset);
            Gizmos.DrawLine(origin - offset, origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength - offset);
            Gizmos.DrawLine(origin + offset, origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength + offset);
            Gizmos.DrawLine(origin + offset, origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength + offset);

            // Draw cone sides with thickness
            Gizmos.DrawLine(origin - offset, origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) - offset);
            Gizmos.DrawLine(origin - offset, origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) - offset);
            Gizmos.DrawLine(origin + offset, origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) + offset);
            Gizmos.DrawLine(origin + offset, origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength / Mathf.Cos(Mathf.Deg2Rad * halfWidth) + offset);

            // Draw lines to connect cone sides to cone base with thickness
            Gizmos.DrawLine(origin - offset, origin - offset);
            Gizmos.DrawLine(origin + offset, origin + offset);
            Gizmos.DrawLine(origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength - offset, origin + Quaternion.Euler(0, -halfWidth, 0) * direction * coneLength + offset);
            Gizmos.DrawLine(origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength - offset, origin + Quaternion.Euler(0, halfWidth, 0) * direction * coneLength + offset);

            // Restore the previous Gizmos color
            Gizmos.color = previousColor;
        }
        #endregion

        #region IDamagable
        public void Hit(int damage)
        {
            HealthPoints -= damage;
            enemyHealthBar.UpdateHealthBar(HealthPoints,MaxHealthPoints);
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (HealthPoints <= 0)
            {
                GetComponent<HealthDropScript>().InstantiateDroppedItem(transform.position);
                Destroy(gameObject);
            }
        }
        #endregion

        #region Animator
        public void EndWalk()
        {
            animator.SetBool(animIDWalking, false);
        }

        public void EndAnticipate()
        {
            animator.SetBool(animIDAnticipate, false);
            animator.SetBool(animIDAttack, true);
        }

        public void DoAttack()
        {
            hasAttacked = true;
        }

        public void EndAttack()
        {   
            hasAttacked = false;
            animator.SetBool(animIDAnticipate, false);
            animator.SetBool(animIDAttack, false);
        }
        #endregion
    }
}
