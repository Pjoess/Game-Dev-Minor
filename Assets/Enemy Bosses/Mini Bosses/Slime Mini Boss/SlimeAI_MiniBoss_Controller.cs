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
        [SerializeField] private float patrolWaitTime = 4f;
        [SerializeField] private float patrolRange = 10f;

        [Header("Patrol")]
        [SerializeField] private bool isPatrolling;

        [Header("Chase")]
        [SerializeField] private AudioSource chaseMusic;
        [SerializeField] private float chaseRange = 15f;
        [SerializeField] private bool isChasingPlayer = false;

        [Header("Attack")]
        [SerializeField] private float attackRange = 4f;
        [SerializeField] private bool isAttacking;

        [Header("Stats")]
        [SerializeField] private int healthPoints;
        [SerializeField] private int maxHealthPoints = 15;

        [Header("Slime Damage")]
        [SerializeField] private int miniBossDamage = 25;
    
        // --- IDamagable --- ///
        public int MaxHealthPoints { get { return maxHealthPoints; } }
        [HideInInspector] public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
    #endregion

    #region Default Functions
        void Awake(){
            miniBossAgent = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<Player>();
            chaseMusic = GetComponent<AudioSource>();
        }

        void Start()
        {
            miniBossAgent.speed = movementSpeed;
            HealthPoints = MaxHealthPoints;
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
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // Check if the player is within the chase range
            if (distanceToPlayer <= chaseRange)
            {
                if (!isChasingPlayer)
                {
                    // Pause all other audio sources
                    PauseAllOtherMusic();
                    
                    // Start playing the chase music only if it's not already playing
                    chaseMusic.Play();
                }
                isChasingPlayer = true;
                miniBossAgent.SetDestination(player.transform.position);
            }
            else
            {
                // Stop the chase music if it's currently playing
                if (isChasingPlayer)
                {
                    chaseMusic.Stop();
                }
                
                // Resume all other audio sources
                ResumeAllOtherMusic();
                
                isChasingPlayer = false;
            }
        }

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

            // Check if the player is within the chase range
            if (distanceToPlayer <= attackRange && isAttacking == false)
            {
                isAttacking = true;
                miniBossAgent.SetDestination(player.transform.position);
                StartCoroutine(AttackWait());
            }
        }

        IEnumerator AttackWait()
    {
        Debug.Log("Attacking Player!");
        player.Hit(miniBossDamage);  
        yield return new WaitForSeconds(3);
        Debug.Log("Slime will attack again...");
        isAttacking = false;
    }
    #endregion

    #region IDamagable
        public void Hit(int damage)
        {
            Debug.Log("Boss receives damage");
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