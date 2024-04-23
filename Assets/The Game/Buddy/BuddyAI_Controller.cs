// --- Simple Behaviour Tree --- //
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class BuddyAI_Controller : MonoBehaviour
{
    #region Variables & References
    [Header("References")]
    private NavMeshAgent buddy;
    private Transform player;
    public GameObject bulletPrefab;
    public LayerMask attackLayer;
    public AudioSource shootSound;

    [Header("Attack")]
    [SerializeField] private int shotsFired;
    [SerializeField] private float shootingRange;
    private float bulletSpeed;
    private float bulletLifetime;
    private float bulletShootHeight;
    private float mortarSpeed;
    private float distanceToMove;

    private Coroutine behaviorCoroutine;
    #endregion

    private void StatsOnAwake()
    {
        shotsFired = 0;
        shootingRange = 10f;
        bulletSpeed = 8f;
        bulletLifetime = 5f;
        bulletShootHeight = 1f;
        mortarSpeed = 5f;
        distanceToMove = 5f;
    }

    #region MonoBehaviour Callbacks
    void Awake()
    {
        StatsOnAwake();
        buddy = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        behaviorCoroutine = StartCoroutine(SimpleBehaviourTree());
    }

    void Update(){
        Transform closestEnemy = FindClosestEnemy();
        ShootMortar(closestEnemy);
    }

    void OnDestroy()
    {
        if (behaviorCoroutine != null)
            StopCoroutine(behaviorCoroutine);
    }
    #endregion

    #region Behavior Tree
    IEnumerator SimpleBehaviourTree()
    {
        while (true)
        {
            // Check if player is within buddy's follow distance
            if (IsPlayerWithinFollowDistance())
            {
                Transform closestEnemy = FindClosestEnemy();

                if (closestEnemy != null)
                {
                    StartShootingRoutine(closestEnemy);
                }
            }
            else
            {
                // Move and Patrol around the player
                Patrol();
            }
            yield return new WaitForSeconds(0.5f); // Adjust frequency of behavior tree updates
        }
    }

    bool IsPlayerWithinFollowDistance()
    {
        return Vector3.Distance(transform.position, player.position) <= buddy.stoppingDistance;
    }
    #endregion

    #region Patrol
    void Patrol()
    {
        if (RandomPoint(player.position, 5f, out Vector3 randomPoint))
        {
            buddy.SetDestination(randomPoint);
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
    #endregion

    #region Shooting Bullet
    // Find closest enemy first
    Transform FindClosestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, shootingRange, attackLayer);

        Transform closestEnemy = null;
        float closestEnemyDistance = Mathf.Infinity;

        foreach (Collider enemyCollider in enemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                Transform enemyTransform = enemyCollider.transform;
                float distanceToEnemy = Vector3.Distance(transform.position, enemyTransform.position);

                if (distanceToEnemy < closestEnemyDistance)
                {
                    closestEnemy = enemyTransform;
                    closestEnemyDistance = distanceToEnemy;
                }
            }
        }
        return closestEnemy;
    }

    // Method to start shooting coroutine
    void StartShootingRoutine(Transform enemyTransform)
    {
        StartCoroutine(ShootRoutine(enemyTransform));
    }
    
    // Coroutine for shooting behavior
    IEnumerator ShootRoutine(Transform enemyTransform)
    {
        if(shotsFired < 3)
        {
            shotsFired++;
            buddy.isStopped = true;
            ShootAtEnemy(enemyTransform);
        }
        else
        {   
            yield return new WaitForSeconds(2f);
            shotsFired = 0;
        }
        buddy.isStopped = false;
    }

    // Method for shooting at the enemy
    void ShootAtEnemy(Transform enemyTransform)
    {
        // Check if the enemyTransform is null or has been destroyed
        if (enemyTransform != null)
        {
            if (Vector3.Distance(transform.position, enemyTransform.position) <= shootingRange)
            {
                // Rotate towards the enemy
                transform.LookAt(enemyTransform);

                Vector3 direction = (enemyTransform.position - transform.position).normalized;

                Vector3 bulletSpawnPosition = transform.position + bulletShootHeight * Vector3.up;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.LookRotation(direction));

                bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

                shootSound.Play();

                Destroy(bullet, bulletLifetime);
            }
        } 
    }
    #endregion

    #region Shooting Mortar
    void ShootMortar(Transform enemyTransform)
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (enemyTransform != null)
            {
                Vector3 spawnPosition = enemyTransform.position + Vector3.up * 5f; // Calculate spawn position 5 units above the enemy
                GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                bullet.transform.localScale += new Vector3(2f, 2f, 2f); // Increase the size of the bulletPrefab by 5 units in all directions
                Destroy(bullet, bulletLifetime);

                StartCoroutine(MoveBulletDownwards(bullet));
            }
        }
    }

    IEnumerator MoveBulletDownwards(GameObject bullet)
    {
        // Check if the bullet object is null
        if (bullet == null)
        {
            yield break; // Exit the coroutine if the bullet is null
        }
        
        Vector3 initialPosition = bullet.transform.position; // Initial position of the bullet
        Vector3 targetPosition = initialPosition - Vector3.up * distanceToMove; // Target position to move downwards
        Quaternion initialRotation = Quaternion.LookRotation(Vector3.down); // Initial rotation of the bullet (pointing downwards)
        bullet.transform.rotation = initialRotation; // Set initial rotation of the bullet

        // Current time elapsed
        float elapsedTime = 0f;

        // Move the bullet downwards over its lifetime
        while (elapsedTime < bulletLifetime)
        {
            if (bullet == null)
            {
                yield break; // Exit the coroutine if the bullet is null
            }
            
            Vector3 newPosition = bullet.transform.position - mortarSpeed * Time.deltaTime * Vector3.up; // Calculate the position to move towards
            bullet.transform.position = newPosition; // Move the bullet downwards
            elapsedTime += Time.deltaTime; // Update elapsed time

            // Wait for the next frame
            yield return null;
        }

        // Ensure the bullet reaches the target position
        if (bullet != null)
        {
            bullet.transform.position = targetPosition;
        }
    }
    #endregion

    #region Toggle Attack Behaviour
    // public void ToggleAttackBehaviour()
    // {
    //     toggleBuddyAttackText.text = toggleAttack ? "Buddy Passive" : "Buddy Aggressive";
    //     toggleAttack = !toggleAttack;
    // }
    #endregion

    #region Drawing Gizmos
    private void OnDrawGizmosSelected()
    {
        if (buddy != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, buddy.radius); // Obstacle avoidance

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootingRange); // Shooting range

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, buddy.stoppingDistance); // Stoppingdistance
        }
    }
    #endregion
}