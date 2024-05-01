using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class SlimeMiniBoss_Agent : MonoBehaviour, IDamageble
    {
        private IBaseNode slimeBT = null;
        public LayerMask attackLayer;
        private NavMeshAgent miniBossAgent;

        [Header("Patrol Center Point")]
        public GameObject patrolCenterPoint;

        [Header("Chase")]
        private float chaseRange = 15f;
        private bool isChasingPlayer = false;

        [Header("Attack")]
        private float attackRange = 5f;
        private float offsetDistance = 3f;
        private bool isAttacking;
        
        // --- IDamagable --- //
        [Header("Stats")]
        private EnemyHealthBar enemyHealthBar;
        public int healthPoints;
        public int maxHealthPoints = 100;
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
    
        [Header("Cone Settings")]
        private float coneWidth = 40f;
        private float coneLength = 5f;
        private float thickness = 2f;

        private void Awake()
        {
            miniBossAgent = GetComponent<NavMeshAgent>();
            enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        }

        void Start()
        {
            HealthPoints = MaxHealthPoints;
            MiniBossSlimeBehaviourTree();
        }

        void Update()
        {
            slimeBT?.Update(); // Update the boss behavior tree
        }
#region Behaviour Tree
        private void MiniBossSlimeBehaviourTree()
        {
            List<IBaseNode> bossNodes = new()
            {
                new AttackPlayerNode(miniBossAgent, attackRange, offsetDistance, attackLayer, coneWidth, coneLength),
                new ChasePlayerNode(miniBossAgent, chaseRange),
            };

            slimeBT = new SelectorNode(bossNodes);
        }
#endregion

#region Cone Raycast
        // Draw Gizmos for cone shape and chase range
        private void OnDrawGizmos()
        {
            // Draw cone shape in Gizmos
            DrawCone(transform.position, transform.forward, coneWidth, coneLength, thickness);

            // Draw chase range sphere
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
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
        // Mini Boss Receive damage
        public void ApplyDamageToMiniBoss() => healthPoints -= 3; // Do damage to boss (with bullets)

        public void Hit(int damage)
        {
            HealthPoints -= damage;
            //ApplyDamageToMiniBoss();
            //Debug.Log("Healthpoints : " + HealthPoints);
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
    }
}
