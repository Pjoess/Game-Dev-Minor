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
    private float bulletSpeed = 8f;
    private float bulletLifetime = 3f;
    private float bulletShootHeight = 1f;

    private Coroutine behaviorCoroutine;
    #endregion

    private void StatsOnAwake()
    {
        shotsFired = 0;
        shootingRange = 10f;
        bulletSpeed = 8f;
        bulletLifetime = 3f;
        bulletShootHeight = 1f;
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

    #region Shooting
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

    #region Toggle Attack Behavior
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