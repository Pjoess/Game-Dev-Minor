using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class SlimeMiniBoss_Agent : MonoBehaviour, IDamageble
    {
        private IBaseNode slimeBT = null;
        public LayerMask attackLayer; // Player
        private NavMeshAgent miniBossAgent;
        private Rigidbody rigidBody;
        private ParticleSystem shockwaveParticleSystem;

        [Header("Patrol Center Point")]
        public GameObject patrolCenterPoint;

        [Header("Chase")]
        private float chaseRange = 20f;

        [Header("Attack")]
        private float attackRange = 19f;
        private float offsetDistance = 1f;
        public static bool hasAttacked = false;
        
        // --- IDamagable --- //
        [Header("Stats")]
        private EnemyHealthBar enemyHealthBar;
        public int healthPoints;
        public int maxHealthPoints = 100;
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
    
        [Header("Cone Settings")]
        private float coneWidth = 50f;
        private float coneLength = 10f;
        private float thickness = 2f;

        [Header("Patrol Settings")]
        private float patrolRadius = 20f;
        private float stopDistance = 4f;

        private Animator animator;
        private int animIDAnticipate;
        private int animIDAttack;

        private void AssignAnimIDs()
        {
            animIDAnticipate = Animator.StringToHash("isAnticipating");
            animIDAttack = Animator.StringToHash("isAttacking");
        }

        private void Awake()
        {
            AssignAnimIDs();
            HealthPoints = MaxHealthPoints;
            animator = GetComponent<Animator>();
            miniBossAgent = GetComponent<NavMeshAgent>();
            enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
            rigidBody = GetComponent<Rigidbody>();
            shockwaveParticleSystem = GetComponentInChildren<ParticleSystem>();
        }

        void Start()
        {
            MiniBossSlimeBehaviourTree();
        }

        void Update()
        {
            slimeBT?.Update(); // Update the boss behavior tree
        }

        #region Behaviour Tree
        private void MiniBossSlimeBehaviourTree()
        {
            //PatrolChaseAttack();
            OnlyAttackAndRotate();
        }

        private void OnlyAttackAndRotate()
        {
            List<IBaseNode> IsPlayerInLineOfSight = new()
            {
                new AttackPlayerOnPositionNode(miniBossAgent, attackRange, offsetDistance, attackLayer, coneWidth, coneLength,animator,animIDAnticipate,animIDAttack),
            };

            slimeBT = new SelectorNode(IsPlayerInLineOfSight);
        }

        // Removed for now, changed to mini slime boss stays on its spot and just looks at the player.
        private void PatrolChaseAttack()
        {
            List<IBaseNode> IsPlayerInLineOfSight = new()
            {
                new ChasePlayerNode(miniBossAgent, chaseRange, stopDistance),
                new AttackPlayerNode(miniBossAgent, attackRange, offsetDistance, attackLayer, coneWidth, coneLength,animator,animIDAnticipate,animIDAttack),
            };

            List<IBaseNode> IsPlayerNotInLineOfSight = new()
            {
                new PatrolNode(miniBossAgent, patrolCenterPoint, patrolRadius, stopDistance, chaseRange),
            };

            List<IBaseNode> Root = new()
            {
                new SequenceNode(IsPlayerInLineOfSight),
                new SequenceNode(IsPlayerNotInLineOfSight),
            };

            slimeBT = new SelectorNode(Root);
        }
        #endregion

        #region Cone Raycast
        // Draw Gizmos for cone shape and chase range
        private void OnDrawGizmos()
        {
            // Draw cone shape in Gizmos
            DrawCone(transform.position, transform.forward, coneWidth, coneLength, thickness);

            // *** Commented so because the mini bose changed to not moving, but only rotating *** //
            // Draw chase range sphere
            // Gizmos.color = Color.blue;
            // Gizmos.DrawWireSphere(transform.position, chaseRange);

            // Draw stop distance
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawWireSphere(transform.position, stopDistance);

            // Draw attack range sphere
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            // Draw patrol radius
            if(patrolCenterPoint != null){
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(patrolCenterPoint.transform.position, patrolRadius);
            }
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
                GetComponent<MemoryDropScipt>().DropItem(transform.position);
                Destroy(gameObject);
            }
        }
        #endregion

        #region Animator
        public void DoShockwaveAttack()
        {
            hasAttacked = true;
            shockwaveParticleSystem.Play();
        }

        public void EndAnticipate()
        {
            animator.SetBool(animIDAttack, true);
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
