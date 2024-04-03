using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI_MiniBoss_Controller : MonoBehaviour
{
public Transform centerPoint; // Center point for patrolling
    public float patrolRange = 5f; // Range of patrolling
    public LayerMask playerLayer; // Layer mask for detecting player
    public float chaseRange = 10f; // Range at which AI starts chasing player
    public float patrolWaitTime = 2f; // Time to wait at patrol destination
    private NavMeshAgent agent;
    private Vector3 originalPosition; // Original position for patrolling
    private SphereCollider centerCollider; // Collider to detect when AI leaves patrolling area

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        centerCollider = centerPoint.GetComponent<SphereCollider>();
        StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        // Check if player is within chase range
        Collider[] colliders = Physics.OverlapSphere(transform.position, chaseRange, playerLayer);
        if (colliders.Length > 0)
        {
            // Player detected, chase and attack
            Transform player = colliders[0].transform;
            agent.SetDestination(player.position);

            // Check if AI is close enough to attack
            if (Vector3.Distance(transform.position, player.position) <= agent.stoppingDistance)
            {
                // Implement attack logic here
                AttackPlayer();
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
}
