using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class BuddyAI_Controller : MonoBehaviour
{
    #region Variables & References
        [Header("Object References")]
        [SerializeField] private NavMeshAgent buddy;
        [SerializeField] private Transform player;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform patrolCenterPoint;
        [SerializeField] private LayerMask attackLayer;
        [SerializeField] private TextMeshProUGUI toggleBuddyAttackText;

        [Header("Movement & Rotation")]
        [SerializeField] private float buddyToPlayerDistance = 6f;
        [SerializeField] private float avoidanceDistance = 7f;
        [SerializeField] private float nextMoveTimer = 2f;

        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 750f;
        [SerializeField] private float maxRotateToAngleMove = 1f;

        [Header("Attack")]
        [SerializeField] private float shootingInterval = 2f;
        [SerializeField] private float shootingRange = 15f;

        [Header("Projectiles")]
        [SerializeField] private float bulletSpeed = 8f;
        [SerializeField] private float bulletLifetime = 3f;
        [SerializeField] private float bulletShootHeight = 1f;
        [SerializeField] private bool toggleAttack;

        // NavMeshAgent AI
        [HideInInspector] private readonly float navMeshAgent_Speed = 8f;
        [HideInInspector] private readonly float navMeshAngular_Speed = 750f;
        [HideInInspector] private readonly float navMeshAcceleration_Speed = 20f;

        // Private Variables & References
        [HideInInspector] private Coroutine shootingRoutine;
    #endregion

    #region Default Functions
        void Awake()
        {
            buddy = GetComponent<NavMeshAgent>();
            patrolCenterPoint = GetComponent<Transform>();

            // Override NavMesh Agent Variables
            buddy.speed = navMeshAgent_Speed;
            buddy.angularSpeed = navMeshAngular_Speed;
            buddy.acceleration = navMeshAcceleration_Speed;
        }

        void Update()
        {
            BuddyToPlayerDistanceCheck();
        }
    #endregion

    #region Buddy Checks
        private void BuddyToPlayerDistanceCheck()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > buddyToPlayerDistance)
            {
                buddy.SetDestination(player.position);
                return;
            }
        }
    #endregion

    #region Buddy Movement
        // Method for calculating an avoidance point to avoid nearby obstacles
        Vector3 CalculateAvoidancePoint()
        {
            Collider[] nearbyObstacles = Physics.OverlapSphere(transform.position, avoidanceDistance, attackLayer);

            foreach (Collider obstacle in nearbyObstacles)
            {
                if (obstacle.transform != transform) // Avoid itself
                {
                    Vector3 avoidanceDirection = (transform.position - obstacle.transform.position).normalized;
                    Vector3 avoidancePoint = transform.position + avoidanceDirection * avoidanceDistance;
                    
                    NavMeshPath path = new NavMeshPath(); // Instantiate NavMeshPath
                    if (NavMesh.CalculatePath(transform.position, avoidancePoint, NavMesh.AllAreas, path))
                    {
                        if (path.corners.Length > 1 && Vector3.Distance(path.corners[1], obstacle.transform.position) > avoidanceDistance)
                        {
                            return path.corners[1];
                        }
                    }
                }
            }
            return Vector3.zero; // No nearby obstacles found
        }

        // Method for finding a random point on the navmesh
        bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }

            result = Vector3.zero;
            return false;
        }

        // Method for moving to the next destination
        void MoveToNextDestination()
        {
            // Start coroutine to wait and then move
            StartCoroutine(WaitAndMove());
        }

        // Coroutine to wait and then move
        IEnumerator WaitAndMove()
        {
            // Wait for 3 seconds
            yield return new WaitForSeconds(nextMoveTimer);

            // Calculate the next move after waiting
            CalculateNextMove();
        }

        // Method for calculating the next move
        void CalculateNextMove()
        {
            Vector3 avoidancePoint = CalculateAvoidancePoint();

            if (avoidancePoint != Vector3.zero)
            {
                buddy.SetDestination(avoidancePoint);
            }
            else
            {
                if (RandomPoint(patrolCenterPoint.position, buddyToPlayerDistance, out Vector3 point))
                {
                    buddy.SetDestination(point);
                }
            }
        }
    #endregion

   #region Buddy Attack
    // Method to check if an enemy is in line of sight
    bool IsInLineOfSight(Transform enemyTransform)
    {
        Vector3 direction = enemyTransform.position - transform.position;

        // Cast a ray towards the enemy
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, Mathf.Infinity, attackLayer))
        {
            if (hit.transform == enemyTransform)
            {
                // Enemy is in line of sight
                return true;
            }
        }
        // Enemy is not in line of sight
        return false;
    }

    // Method for shooting at the enemy
    void ShootAtEnemy(Transform enemyTransform)
    {
        // Check if the enemy is within shooting range and in line of sight
        if (Vector3.Distance(transform.position, enemyTransform.position) <= shootingRange && IsInLineOfSight(enemyTransform))
        {
            Vector3 direction = (enemyTransform.position - transform.position).normalized;
            Vector3 bulletSpawnPosition = transform.position + bulletShootHeight * Vector3.up;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.LookRotation(direction)); // Point bullet in the direction
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
            Destroy(bullet, bulletLifetime);
        }
    }


    IEnumerator ShootAtEnemyRoutine()
    {
        while (true)
        {
            // Find all enemies within attack range
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, shootingRange, attackLayer);

            // Track the nearest enemy and its distance
            Transform nearestEnemy = null;
            float nearestEnemyDistance = Mathf.Infinity;

            // Iterate through all enemies to find the nearest one
            foreach (Collider collider in hitColliders)
            {
                // Calculate the distance to the current enemy
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

                // Update the nearest enemy if the current one is closer
                if (distanceToEnemy < nearestEnemyDistance)
                {
                    nearestEnemy = collider.transform;
                    nearestEnemyDistance = distanceToEnemy;
                }
            }

            // If a nearest enemy is found, shoot at it
            if (nearestEnemy != null)
            {
                ShootAtEnemy(nearestEnemy);
            }

            yield return new WaitForSeconds(shootingInterval);
        }
    }
    #endregion

    #region Toggle Buddy Attack
    public void ToggleAttackBehaviour()
    {
        toggleBuddyAttackText.text = toggleAttack ? "Buddy Passive" : "Buddy Aggressive";
        toggleAttack = !toggleAttack;
        // Start or stop the shooting routine based on toggleAttack
        if (toggleAttack && shootingRoutine == null)
        {
            shootingRoutine = StartCoroutine(ShootAtEnemyRoutine());
        }
        else if (!toggleAttack && shootingRoutine != null)
        {
            StopCoroutine(shootingRoutine);
            shootingRoutine = null;
        }
    }
    #endregion

    #region Drawing Gizmos for checking Range
        private void OnDrawGizmosSelected()
        {
            // Draw a wire sphere to represent the buddy to player range
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, buddyToPlayerDistance);

            // Draw a wire sphere to represent the attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootingRange);

            // Draw a wire sphere to represent the avoidance range
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, avoidanceDistance);
        }
    #endregion
}
