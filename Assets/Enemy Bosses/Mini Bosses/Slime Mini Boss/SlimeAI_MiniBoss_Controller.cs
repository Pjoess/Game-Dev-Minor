using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI_MiniBoss_Controller : MonoBehaviour, IDamageble
{
    public Transform centerPoint; // Center point for patrolling
    public float patrolRange = 5f; // Range of patrolling
    public LayerMask playerLayer; // Layer mask for detecting player
    public float chaseRange = 10f; // Range at which AI starts chasing player
    public float patrolWaitTime = 2f; // Time to wait at patrol destination
    public AudioClip chaseMusic; // Music to play while chasing
    private NavMeshAgent agent;
    private Vector3 originalPosition; // Original position for patrolling
    private SphereCollider centerCollider; // Collider to detect when AI leaves patrolling area
    private AudioSource audioSource;
    private bool isChasingPlayer = false;

    // From IDamagable
    public int MaxHealthPoints { get; }
    public int HealthPoints { get; set; }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        centerCollider = centerPoint.GetComponent<SphereCollider>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        CheckChaseRange();
    }

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

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // Check if AI is outside the patrolling area
            if (!centerCollider.bounds.Contains(transform.position))
            {
                // AI is outside the patrolling area, return to center
                agent.SetDestination(centerPoint.position);
                yield return new WaitForSeconds(patrolWaitTime); // Wait for AI to reach the center
                continue; // Skip the rest of the loop iteration
            }

            // Randomly select a point within patrol range around center point
            Vector3 randomPoint = centerPoint.position + Random.insideUnitSphere * patrolRange;
            NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRange, 1);
            Vector3 finalPoint = hit.position;

            // Set destination to the selected point
            agent.SetDestination(finalPoint);

            // Wait for patrolWaitTime seconds
            yield return new WaitForSeconds(patrolWaitTime);
        }
    }

    void AttackPlayer()
    {
        // Implement attack logic here
        Debug.Log("Attacking Player!");
    }

    public void Hit(int damage)
    {
        
    }
}
