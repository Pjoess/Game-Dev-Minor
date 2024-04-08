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
        [SerializeField] private float buddyToPlayerDistance = 8f;
        [SerializeField] private float avoidanceDistance = 6f;
        [SerializeField] private float isStandingStillTimer = 2f;
        [HideInInspector] private bool isStandingStill = false;
        [SerializeField] private float nextMoveTime = 2f;

        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 750f;
        [SerializeField] private float maxRotationAngle = 6f;

        [Header("Attack")]
        [SerializeField] private float shootingInterval = 0.5f;
        [SerializeField] private float shootingRange = 10f;

        [Header("Projectiles")]
        [SerializeField] private float bulletSpeed = 8f;
        [SerializeField] private float bulletLifetime = 3f;
        [SerializeField] private bool toggleAttack;

        // NavMeshAgent AI
        [HideInInspector] private float navMeshAgent_Speed = 5f;
        [HideInInspector] private float navMeshAngular_Speed = 750f;
        [HideInInspector] private float navMeshAcceleration_Speed = 20f;

        // Private Variables & References
        [HideInInspector] private Coroutine shootingRoutine;
        [HideInInspector] private Rigidbody rigidBody;
    #endregion

    #region Default Functions
        void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            buddy = GetComponent<NavMeshAgent>();
            // Override NavMesh Agent Variables
            buddy.speed = navMeshAgent_Speed;
            buddy.angularSpeed = navMeshAngular_Speed;
            buddy.acceleration = navMeshAcceleration_Speed;
        }

        void Start()
        {
            ToggleShooting();
        }

        private void ToggleShooting()
        {
            if (toggleAttack)
            {
                shootingRoutine = StartCoroutine(ShootAtEnemyRoutine());
            }
        }

        void Update()
        {
            BuddyChecker();
        }
    #endregion

    #region Buddy Checks
        private void BuddyChecker()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > buddyToPlayerDistance)
            {
                buddy.SetDestination(player.position);
                return;
            }

            if (isStandingStill)
            {
                StandStillTimer();
            }
            else if (buddy.remainingDistance <= buddy.stoppingDistance)
            {
                if (!isStandingStill)
                {
                    StartCoroutine(StandStillCoroutine());
                }
            }
        }
    #endregion

    #region Buddy Movement
        // Method for calculating an avoidance point to avoid the player
        Vector3 CalculateAvoidancePoint()
        {
            Vector3 directionToPlayer = player.position - transform.position;
            Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, directionToPlayer).normalized;
            Vector3 avoidancePoint = player.position + avoidanceDirection * avoidanceDistance;

            NavMeshPath path = new NavMeshPath(); // Instantiate NavMeshPath
            if (NavMesh.CalculatePath(transform.position, avoidancePoint, NavMesh.AllAreas, path))
            {
                if (path.corners.Length > 1 && Vector3.Distance(path.corners[1], player.position) > avoidanceDistance)
                {
                    return path.corners[1];
                }
            }
            return Vector3.zero;
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
            Invoke(nameof(CalculateNextMove), nextMoveTime);
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

        // Timer for standing still
        void StandStillTimer()
        {
            isStandingStillTimer -= Time.deltaTime;
            if (isStandingStillTimer <= 0f)
            {
                isStandingStill = false;
                MoveToNextDestination();
            }
        }

        // Coroutine for standing still for a certain time
        IEnumerator StandStillCoroutine()
        {
            isStandingStill = true;
            yield return new WaitForSeconds(isStandingStillTimer);
            isStandingStill = false;
            MoveToNextDestination();
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
        void ShootAtEnemy(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            
            if (Quaternion.Angle(transform.rotation, lookRotation) < maxRotationAngle)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
                Destroy(bullet, bulletLifetime);
            }
        }

        // Coroutine for shooting at the enemy
        IEnumerator ShootAtEnemyRoutine()
        {
            while (true)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, shootingRange, attackLayer);
                foreach (Collider collider in hitColliders)
                {
                    // Check if the enemy is within line of sight
                    if (IsInLineOfSight(collider.transform))
                    {
                        ShootAtEnemy(collider.transform.position);
                        buddy.SetDestination(collider.transform.position);
                    }
                }
                yield return new WaitForSeconds(shootingInterval);
            }
        }
    #endregion

    #region Toggle Buddy Attack
        public void ToggleBehaviour()
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
}
