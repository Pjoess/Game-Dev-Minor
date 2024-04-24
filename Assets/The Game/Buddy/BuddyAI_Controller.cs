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
        public GameObject mortarPrefab;
        public LayerMask attackLayer;
        public AudioSource shootSound;
        private Rigidbody rigidBody;

        [Header("Attack")]
        [SerializeField] private int shotsFired;
        [SerializeField] private float shootingRange;
        private float bulletSpeed;
        private float bulletLifetime;
        private float bulletShootHeight;
        private float mortarSpeed;
        private float distanceToMove;
        private float mortarSpawnHeight;

        [Header("Cooldown")]
        [SerializeField] private TMP_Text buddyCooldownText;
        [SerializeField] private float mortarCooldownTime = 3f;
        private float nextMortarTime = 0f;

        private Animator animator;
        [HideInInspector] public int animIDWalk;
        [HideInInspector] public int animIDShooting;
        [HideInInspector] public int animIDShootingMortar;
    #endregion

    private void AssignAnimIDs()
    {
        animIDWalk = Animator.StringToHash("isWalking");
        animIDShooting = Animator.StringToHash("isShooting");
        animIDShootingMortar = Animator.StringToHash("isShootingMortar");
    }

    private void DefaultStatsOnAwake()
    {
        // Initialize variables to default values
        shotsFired = 0;
        shootingRange = 10f;
        bulletSpeed = 8f;
        bulletLifetime = 5f;
        bulletShootHeight = 1f;
        mortarSpeed = 5f;
        distanceToMove = 5f;
        mortarSpawnHeight = 8f;
    }

    #region MonoBehaviour Callbacks
    void Awake()
    {
        AssignAnimIDs();
        DefaultStatsOnAwake();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        // Assign references
        buddy = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Ensure player exists
    }

    void Start()
    {
        // Ensure proper initialization before starting behavior tree
        if (buddy != null && player != null)
        {
            StartCoroutine(SimpleBehaviourTree());
        }
        else
        {
            Debug.LogError("Buddy or player reference is null. Check initialization.");
        }
    }

    void Update()
    {
        // Check if the object is not moving
        if (buddy.velocity.magnitude <= 0.2f)
        {
            animator.SetBool(animIDWalk, false);
        }
        else
        {
            animator.SetBool(animIDWalk, true);
        }
        ShootMortar();     
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
                WalkToPlayerRandom();
            }
            yield return new WaitForSeconds(0.5f); // Adjust frequency of behavior tree updates
        }
    }

    bool IsPlayerWithinFollowDistance()
    {
        // Ensure player reference is not null before using it
        return player != null && Vector3.Distance(transform.position, player.position) <= buddy.stoppingDistance;
    }
    #endregion

    #region Walk and Patrol
    void WalkToPlayerRandom()
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

    void StartShootingRoutine(Transform enemyTransform)
    {
        StartCoroutine(ShootRoutine(enemyTransform));
    }

    IEnumerator ShootRoutine(Transform enemyTransform)
    {
        if (shotsFired < 3)
        {
            animator.SetBool(animIDShooting, true);
            shotsFired++;
            buddy.isStopped = true;
            ShootAtEnemy(enemyTransform);
            
        }
        else
        {
            animator.SetBool(animIDShooting, false);
            animator.SetBool(animIDWalk, false);
            yield return new WaitForSeconds(2f);
            shotsFired = 0;
        }
        buddy.isStopped = false;
    }

    void ShootAtEnemy(Transform enemyTransform)
    {
        if (enemyTransform != null)
        {
            // Ensure necessary components are not null before using them
            if (bulletPrefab != null && shootSound != null)
            {
                if (Vector3.Distance(transform.position, enemyTransform.position) <= shootingRange)
                {
                    transform.LookAt(enemyTransform);

                    Vector3 direction = (enemyTransform.position - transform.position).normalized;
                    Vector3 bulletSpawnPosition = transform.position + bulletShootHeight * Vector3.up;

                    GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.LookRotation(direction));

                    Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
                    if (bulletRigidbody != null)
                    {
                        bulletRigidbody.velocity = direction * bulletSpeed;
                    }

                    shootSound.Play();
                    Destroy(bullet, bulletLifetime);
                }
            }
            else
            {
                Debug.LogError("Bullet prefab or shoot sound reference is null.");
            }
        }
    }
    #endregion

    #region Shooting Mortar
    private void ShootMortar()
    {
        if (Time.time >= nextMortarTime)
        {
            buddyCooldownText.text = "Mortar Ready! - (Press RMB)";
            
            if (Input.GetMouseButtonDown(1))
            {
                Transform closestEnemy = FindClosestEnemy();
                if (closestEnemy != null && mortarPrefab != null)
                {
                    animator.SetBool(animIDShootingMortar, true);
                    Vector3 spawnPosition = closestEnemy.position + Vector3.up * mortarSpawnHeight;
                    GameObject mortar = Instantiate(mortarPrefab, spawnPosition, Quaternion.identity);
                    mortar.transform.localScale += new Vector3(2f, 2f, 2f);
                    Destroy(mortar, bulletLifetime);

                    StartCoroutine(MoveBulletDownwards(mortar));

                    nextMortarTime = Time.time + mortarCooldownTime;
                    animator.SetBool(animIDShootingMortar, false);
                }
            }
            
        }
        else
        {
            float remainingTime = nextMortarTime - Time.time;
            buddyCooldownText.text = "Cooldown: " + Mathf.CeilToInt(remainingTime) + "s";
        }
    }

    IEnumerator MoveBulletDownwards(GameObject mortar)
    {
        if (mortar == null)
        {
            yield break;
        }

        Vector3 initialPosition = mortar.transform.position;
        Vector3 targetPosition = initialPosition - Vector3.up * distanceToMove;
        Quaternion initialRotation = Quaternion.LookRotation(Vector3.down);
        mortar.transform.rotation = initialRotation;

        float elapsedTime = 0f;

        while (elapsedTime < bulletLifetime)
        {
            if (mortar == null)
            {
                yield break;
            }

            Vector3 newPosition = mortar.transform.position - mortarSpeed * Time.deltaTime * Vector3.up;
            mortar.transform.position = newPosition;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if (mortar != null)
        {
            mortar.transform.position = targetPosition;
        }
    }
    #endregion

    #region Drawing Gizmos
    private void OnDrawGizmosSelected()
    {
        if (buddy != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, buddy.radius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootingRange);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, buddy.stoppingDistance);
        }
    }
    #endregion
}
