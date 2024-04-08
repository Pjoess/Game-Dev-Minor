using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI_MiniBoss_Controller : MonoBehaviour, IDamageble
{
    #region Variables & References
        [Header("NavMesh Agent")]
        [HideInInspector] private NavMeshAgent miniBossAgent;
        [HideInInspector] private Vector3 originalPosition;

        [Header("Object References")]
        [SerializeField] private Transform player;
        [SerializeField] private GameObject patrolCenterPoint;

        [Header("Movement")]
        [SerializeField] private float movementSpeed = 2f;
        [SerializeField] private float patrolWaitTime = 4f;
        [SerializeField] private float patrolRange = 10f;
        [SerializeField] private float chaseRange = 15f;
        [SerializeField] private bool isChasingPlayer = false;
        [SerializeField] private bool isPatrolling = false;

        [Header("Movement")]
        [SerializeField] private float attackRange = 4f;
        [SerializeField] private bool isAttacking = false;

        [Header("Stats")]
        [SerializeField] private int healthPoints;
        [SerializeField] private int maxHealthPoints = 15;
    

        // --- IDamagable --- ///
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        [HideInInspector] public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
    #endregion

    #region Default Functions
        void Start()
        {
            miniBossAgent = GetComponent<NavMeshAgent>();
            miniBossAgent.speed = movementSpeed;
            originalPosition = transform.position;
            HealthPoints = MaxHealthPoints;
            
            // Start patrolling
            StartCoroutine(PatrolRoutine());
        }

        void Update()
        {
            CheckChasePlayer();
            AttackPlayer();
        }
    #endregion
    
    #region Movement
        IEnumerator PatrolRoutine()
        {
            while (true)
            {
                if (!isChasingPlayer)
                {
                    // Set destination and wait
                    Vector3 randomDestination = GetRandomDestination();
                    miniBossAgent.SetDestination(randomDestination);

                    yield return new WaitForSeconds(patrolWaitTime);

                    miniBossAgent.ResetPath(); // Reset patrolling
                }

                yield return null;
            }
        }

        Vector3 GetRandomDestination()
        {
            // Get the radius of the sphere collider attached to the center point
            float patrolRadius = patrolCenterPoint.GetComponent<SphereCollider>().radius;
            float randomAngle = Random.Range(0f, Mathf.PI * 2f); // Calculate a random angle

            // Calculate the random direction within the circle around the patrol center point
            Vector3 randomDirection = new Vector3(Mathf.Sin(randomAngle), 0f, Mathf.Cos(randomAngle)) * Random.Range(0f, patrolRadius);
            Vector3 randomDestination = patrolCenterPoint.transform.position + randomDirection;

            // Use overlapsphere to find a valid position within the NavMesh
            if (NavMesh.SamplePosition(randomDestination, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return randomDestination; // If no valid position is found, return the original random destination
        }
    #endregion

    #region Chasing
        void CheckChasePlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if the player is within the chase range
            if (distanceToPlayer <= chaseRange)
            {
                isChasingPlayer = true;
                miniBossAgent.SetDestination(player.position);
            }
            else
            {
                isChasingPlayer = false;
            }
        }
    #endregion

    #region Attack
        void AttackPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if the player is within the chase range
            if (distanceToPlayer <= attackRange)
            {
                isAttacking = true;
                miniBossAgent.SetDestination(player.position);
                Debug.Log("Attacking Player!");
            }
            else
            {
                isAttacking = false;
            }
        }

        public void Hit(int damage)
        {
            Debug.Log("Boss hit");
            HealthPoints -= damage;
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

    #region Drawing Gizmos for checking Range
        private void OnDrawGizmosSelected()
        {
            // Draw a wire sphere to represent the patrol range
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, patrolRange);

            // Draw a wire sphere to represent the chase range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);

            // Draw a wire sphere to represent the attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    #endregion
}