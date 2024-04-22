// --- Simple Behaviour Tree --- //
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class BuddyAI_Controller : MonoBehaviour
{
    #region Variables & References
    [Header("Object References")]
    private NavMeshAgent buddy;
    private Transform player;
    public GameObject bulletPrefab;
    public LayerMask attackLayer;
    public AudioSource shootSound;

    [Header("Movement & Rotation")]
    public float shootingInterval = 2f;
    public float shootingRange = 15f;
    public float bulletSpeed = 8f;
    public float bulletLifetime = 3f;
    public float bulletShootHeight = 1f;

    private Coroutine behaviorCoroutine;

    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
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
                // Check if enemy is within shooting range and line of sight
                Collider[] enemies = Physics.OverlapSphere(transform.position, shootingRange, attackLayer);
                foreach (Collider enemy in enemies)
                {
                    if (enemy.CompareTag("Enemy"))
                    {
                        ShootAtEnemy(enemy.transform);
                    }
                }
            }
            else
            {
                // Patrol around the player
                Patrol();
            }

            yield return new WaitForSeconds(0.5f); // Adjust frequency of behavior tree updates
        }
    }

    bool IsPlayerWithinFollowDistance()
    {
        return Vector3.Distance(transform.position, player.position) <= buddy.stoppingDistance;
    }

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

    void ShootAtEnemy(Transform enemyTransform)
    {
        if (Vector3.Distance(transform.position, enemyTransform.position) <= shootingRange)
        {
            Vector3 direction = (enemyTransform.position - transform.position).normalized;

            Vector3 bulletSpawnPosition = transform.position + bulletShootHeight * Vector3.up;

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.LookRotation(direction));

            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

            shootSound.Play();
            
            Destroy(bullet, bulletLifetime);
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