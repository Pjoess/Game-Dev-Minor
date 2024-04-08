using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI_MiniBoss_Controller : MonoBehaviour, IDamageble
{
    #region Variables & References
        [Header("Object References")]
        [SerializeField] private Transform patrolCenterPoint;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private SphereCollider patrolCenterPointRadiusCollider;
        
        [Header("Audio References")]
        [SerializeField] private AudioClip chaseMusic;
        [SerializeField] private AudioSource audioSource;

        [Header("NavMesh Agent")]
        [HideInInspector] private NavMeshAgent agent;
        [HideInInspector] private Vector3 originalPosition;

        [Header("Movement")]
        [SerializeField] private float patrolRange = 5f;
        [SerializeField] private float patrolWaitTime = 2f;
        [SerializeField] private float chaseRange = 10f;
        [SerializeField] private bool isChasingPlayer = false;

        [Header("Stats")]
        [SerializeField] private int healthPoints;
        [SerializeField] private int maxHealthPoints = 15;
    #endregion

    // From IDamagable
    public int MaxHealthPoints { get { return maxHealthPoints; } }
    [HideInInspector] public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }

    #region Default Functions
        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            audioSource = GetComponent<AudioSource>();
            patrolCenterPointRadiusCollider = patrolCenterPoint.GetComponent<SphereCollider>();
        }

        void Start()
        {
            originalPosition = transform.position;
            HealthPoints = MaxHealthPoints;
        }

        void Update()
        {
            CheckChaseRange();
            CheckDeath();
            StartCoroutine(PatrolRoutine());
        }
    #endregion

    #region Checks
        private void CheckChaseRange()
        {
            // Check if player is within chase range
            Collider[] colliders = Physics.OverlapSphere(transform.position, chaseRange, playerLayer);
            if (colliders.Length > 0)
            {
                // Player detected, chase and attack
                Transform player = colliders[0].transform;
                agent.SetDestination(player.position);
                if (!isChasingPlayer) // If not already chasing
                {
                    isChasingPlayer = true;
                    audioSource.clip = chaseMusic;
                    audioSource.Play();
                }

                // Check if AI is close enough to attack
                if (Vector3.Distance(transform.position, player.position) <= agent.stoppingDistance)
                {
                    // Implement attack logic here
                    AttackPlayer();
                }
            }
            else
            {
                if (isChasingPlayer) // If stopped chasing
                {
                    isChasingPlayer = false;
                    audioSource.Stop();
                }
            }
        }

        private void CheckDeath()
        {
            if(HealthPoints <= 0)
            {
                GetComponent<MemoryDropScipt>().DropItem(transform.position);
                Destroy(gameObject);
            }
        }
    #endregion

    #region Patrol
        IEnumerator PatrolRoutine()
        {
            while (true)
            {
                // Check if AI is outside the patrolling area
                if (!patrolCenterPointRadiusCollider.bounds.Contains(transform.position))
                {
                    // AI is outside the patrolling area, return to center
                    agent.SetDestination(patrolCenterPoint.position);
                    yield return new WaitForSeconds(patrolWaitTime); // Wait for AI to reach the center
                    continue; // Skip the rest of the loop iteration
                }

                // Randomly select a point within patrol range around center point
                Vector3 randomPoint = patrolCenterPoint.position + Random.insideUnitSphere * patrolRange;
                NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRange, 1);
                Vector3 finalPoint = hit.position;

                // Set destination to the selected point
                agent.SetDestination(finalPoint);

                // Wait for patrolWaitTime seconds
                yield return new WaitForSeconds(patrolWaitTime);
            }
        }
    #endregion

    #region Attack
        void AttackPlayer()
        {
            // Implement attack logic here
            Debug.Log("Attacking Player!");
        }

        public void Hit(int damage)
        {
            Debug.Log("Boss hit");
            HealthPoints -= damage;
        }
    #endregion
}
