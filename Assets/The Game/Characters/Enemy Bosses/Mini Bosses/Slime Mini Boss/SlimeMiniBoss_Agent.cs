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
        private GameObject patrolCenterPoint;

        [Header("Chase")]
        private float chaseRange = 15f;
        private bool isChasingPlayer = false;

        [Header("Attack")]
        private float attackRange = 3f;
        private float offsetDistance = 3f;
        private bool isAttacking;
        
        // --- IDamagable --- //
        [Header("Stats")]
        public EnemyHealthBar enemyHealthBar;
        public int healthPoints;
        public int maxHealthPoints = 100;
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
        
        [Header("Slime Damage")]
        private int miniBossDamage = 25;
    
        [Header("Cone Settings")]
        private float coneWidth = 30f;
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

            // Check for objects in a cone shape
            CheckConeRaycast(transform.forward, coneWidth, coneLength);
        }

        private void MiniBossSlimeBehaviourTree()
        {
            List<IBaseNode> bossNodes = new()
            {
                new ChasePlayerNode(miniBossAgent, chaseRange),
                new AttackPlayerNode(miniBossAgent, attackRange, offsetDistance, miniBossDamage, attackLayer),
            };

            slimeBT = new SelectorNode(bossNodes);
        }

        // Method to check for objects in a cone-shaped raycast
        private void CheckConeRaycast(Vector3 direction, float coneWidth, float coneLength)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, coneLength);

            foreach (RaycastHit hit in hits)
            {
                // Check if the object is within the cone width
                Vector3 directionToHit = hit.point - transform.position;
                float angle = Vector3.Angle(direction, directionToHit);
                if (angle <= coneWidth / 2f)
                {
                    // The hit object is within the cone
                    Debug.Log("Object detected within cone: " + hit.collider.name);
                }
            }
        }

        // Draw Gizmos for cone shape
        private void OnDrawGizmos()
        {
            // Draw cone shape in Gizmos
            DrawCone(transform.position, transform.forward, coneWidth, coneLength, thickness);
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


        // Mini Boss Receive damage
        public void ApplyDamageToMiniBoss() => healthPoints -= 3; // Do damage to boss (with bullets)

        #region IDamagable
        public void Hit(int damage)
        {
            HealthPoints -= damage;
            Debug.Log("" + healthPoints);

            ApplyDamageToMiniBoss();
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
