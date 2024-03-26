using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    // References
    public NavMeshAgent agent;
    public Transform player;
    public GameObject bulletPrefab;
    public Transform centrePoint;
    public LayerMask layerMask;

    // Public variables
    public float range = 5f;
    public float avoidanceDistance = 5f;
    public float bulletSpeed = 6f;
    public float bulletLifetime = 3f;
    public float standStillTime = 1f;
    public float nextMoveTime = 5f;
    public float shootingInterval = 1f;
    public float shootingRange = 5f;

    // Private variables
    private bool isStandingStill = false;
    private Coroutine shootingRoutine;

    // Rotation variables
    public float rotationSpeed = 750f;
    public float maxRotationAngle = 5f;

    // Awake method called when the script instance is being loaded
    void Awake()
    {
        // Getting the NavMeshAgent component attached to the same GameObject
        agent = GetComponent<NavMeshAgent>();
    }


    // Start method called when the script is initialized
    void Start()
    {
        shootingRoutine = StartCoroutine(ShootAtEnemyRoutine());
    }

    // Update method called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > range)
        {
            agent.SetDestination(player.position);
            return;
        }

        if (isStandingStill)
        {
            StandStillTimer();
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isStandingStill)
            {
                StartCoroutine(StandStillCoroutine());
            }
        }
    }

    // Timer for standing still
    void StandStillTimer()
    {
        standStillTime -= Time.deltaTime;
        if (standStillTime <= 0f)
        {
            isStandingStill = false;
            MoveToNextDestination();
        }
    }

    // Coroutine for standing still for a certain time
    IEnumerator StandStillCoroutine()
    {
        isStandingStill = true;
        yield return new WaitForSeconds(standStillTime);
        isStandingStill = false;
        MoveToNextDestination();
    }

    // Coroutine for shooting at the enemy
    IEnumerator ShootAtEnemyRoutine()
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, shootingRange, layerMask);
            foreach (Collider collider in hitColliders)
            {
                ShootAtEnemy(collider.transform.position);
                agent.SetDestination(collider.transform.position);
            }
            yield return new WaitForSeconds(shootingInterval);
        }
    }

    // Method for shooting at the enemy
    void ShootAtEnemy(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        if (Quaternion.Angle(transform.rotation, lookRotation) < maxRotationAngle)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
            Destroy(bullet, bulletLifetime);
        }
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
            agent.SetDestination(avoidancePoint);
        }
        else
        {
            if (RandomPoint(centrePoint.position, range, out Vector3 point))
            {
                agent.SetDestination(point);
            }
        }
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

    // Method for calculating an avoidance point to avoid the player
    Vector3 CalculateAvoidancePoint()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, directionToPlayer).normalized;
        Vector3 avoidancePoint = player.position + avoidanceDirection * avoidanceDistance;

        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, avoidancePoint, NavMesh.AllAreas, path))
        {
            if (path.corners.Length > 1 && Vector3.Distance(path.corners[1], player.position) > avoidanceDistance)
            {
                return path.corners[1];
            }
        }

        return Vector3.zero;
    }
}
