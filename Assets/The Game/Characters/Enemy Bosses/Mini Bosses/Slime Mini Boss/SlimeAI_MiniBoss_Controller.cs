using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI_MiniBoss_Controller : MonoBehaviour, IDamageble
{
    #region Variables & References
        [Header("NavMesh Agent")]
        [HideInInspector] private NavMeshAgent miniBossAgent;

        [Header("Object References")]
        private Player player;
        [SerializeField] private GameObject patrolCenterPoint;

        [Header("Movement")]
        [SerializeField] private float movementSpeed = 2f;

        [Header("Patrol")]
        [SerializeField] private float patrolWaitTime = 4f;
        [SerializeField] private float patrolRange = 10f;
        [SerializeField] private bool isPatrolling;

        [Header("Chase")]
        [SerializeField] private AudioSource chaseMusic;
        [SerializeField] private float chaseRange = 15f;
        [SerializeField] private bool isChasingPlayer = false;

        [Header("Attack")]
        [SerializeField] private float attackRange = 3f;
        [SerializeField] private float offsetDistance = 3f;
        [SerializeField] private bool isAttacking;
        
        // --- IDamagable --- //
        [Header("Stats")]
        [SerializeField] private int healthPoints;
        [SerializeField] private int maxHealthPoints = 40;
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        [HideInInspector] public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
        
        [Header("Slime Damage")]
        [SerializeField] private int miniBossDamage = 25;
        
        public EnemyHealthBar enemyHealthBar;

        [Header("Testing Purpose")]
        [SerializeField] private Vector3 originalMiniBossScale;
    #endregion

    #region Default Functions
        void Awake(){
            miniBossAgent = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<Player>();
            enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        }

        void Start()
        {
            miniBossAgent.speed = movementSpeed;
            HealthPoints = MaxHealthPoints;
            originalMiniBossScale = transform.localScale;

            StartCoroutine(PatrolRoutine()); // Start and Always patrol by default
        }

        void Update()
        {
            CheckChasePlayer(); // Check if enemy is chasing
            AttackPlayer(); // Check everytime if enemy is not attacking
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
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= chaseRange)
            {
                if (!isChasingPlayer)
                {
                    PauseAllOtherMusic();
                    chaseMusic.Play();
                }
                isChasingPlayer = true;

                miniBossAgent.SetDestination(player.transform.position);
            }
            else
            {
                if (isChasingPlayer)
                {
                    chaseMusic.Stop();
                    ResumeAllOtherMusic();
                }
                
                
                isChasingPlayer = false;
            }
        }
    #endregion
        
    #region Music & Audio
        void PauseAllOtherMusic()
        {
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in allAudioSources)
            {
                if (audioSource != chaseMusic)
                {
                    audioSource.Pause();
                }
            }
        }

        void ResumeAllOtherMusic()
        {
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in allAudioSources)
            {
                if (audioSource != chaseMusic)
                {
                    audioSource.UnPause();
                }
            }
        }
    #endregion

    #region Attack
        void AttackPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // Check if the player is within the chase range and did not attack yet
            if (distanceToPlayer <= attackRange && isAttacking == false)
            {
                isAttacking = true;
                StartCoroutine(AttackAndWait());
            }
        }

        IEnumerator AttackAndWait()
        {
            yield return new WaitForSeconds(1f); // Wait before starting an attack
            GetComponent<MeshRenderer>().material.color = new Color32(255, 153, 51, 255); // orange
            yield return new WaitForSeconds(1f); // Wait before almost attacking

            // Check again if the Player is still within its range to attack
            if(Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                GetComponent<MeshRenderer>().material.color = new Color32(255, 0, 0, 255); // Red
                yield return new WaitForSeconds(0.5f);
                ScreenShakeManager.Instance.ShakeCamera(5, 1);
                player.Hit(miniBossDamage); // Boss takes damage from player sword
                yield return new WaitForSeconds(2f); // Attack again after amount of seconds
            }
            GetComponent<MeshRenderer>().material.color = new Color32(255, 235, 8, 255); // Yellow
            isAttacking = false;
        }
    #endregion

    // Mini Boss Receive damage
    public void ApplyDamageToMiniBoss() => healthPoints -= 3; // Do damage to boss (with bullets)

    #region IDamagable
        public void Hit(int damage)
        {
            HealthPoints -= damage;
            ApplyDamageToMiniBoss();
            enemyHealthBar.UpdateHealthBar(HealthPoints,MaxHealthPoints);
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (HealthPoints <= 0)
            {
                GetComponent<MemoryDropScipt>().DropItem(transform.position);
                ResumeAllOtherMusic();
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
            Gizmos.DrawWireSphere(transform.position + transform.forward * offsetDistance, attackRange);
        }
    #endregion
}