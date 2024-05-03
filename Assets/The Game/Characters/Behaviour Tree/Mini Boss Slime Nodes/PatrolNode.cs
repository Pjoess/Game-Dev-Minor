using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class PatrolNode : IBaseNode
    {
        private NavMeshAgent agent;
        private GameObject patrolCenter;
        private float patrolRadiusSquared; // Use squared radius for distance comparison
        private float stopDistanceSquared; // Use squared distance for distance comparison
        private float chaseRangeSquared; // Use squared range for distance comparison
        private Vector3 currentDestination;
        private float patrolTimer = 0f;
        private float patrolInterval = 2f;

        // --- Caching --- //
        private Quaternion lookRotation;
        Vector3 directionToDestination;
        Vector3 randomDirection;

        public PatrolNode(NavMeshAgent agent, GameObject patrolCenter, float patrolRadius, float stopDistance, float chaseRange)
        {
            this.agent = agent;
            this.patrolCenter = patrolCenter;
            this.patrolRadiusSquared = patrolRadius * patrolRadius;
            this.stopDistanceSquared = stopDistance * stopDistance;
            this.chaseRangeSquared = chaseRange * chaseRange;
            currentDestination = GetRandomDestination();
        }

        public virtual bool Update()
        {
            patrolTimer += Time.deltaTime;
            if (Vector3.SqrMagnitude(agent.transform.position - currentDestination) <= stopDistanceSquared || patrolTimer >= patrolInterval)
            {
                currentDestination = GetRandomDestination();
                agent.SetDestination(currentDestination);
                patrolTimer = 0f;
            }

            // No need to rotate if the agent has reached its destination
            if (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
            {
                RotateTowardsDestination();
            }
            
            return true;
        }

        private void RotateTowardsDestination()
    {
        directionToDestination = currentDestination - agent.transform.position;
        if (directionToDestination.sqrMagnitude > Mathf.Epsilon) // Check if the magnitude is greater than epsilon
        {
            lookRotation = Quaternion.LookRotation(directionToDestination);
            agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, lookRotation, agent.angularSpeed * Time.deltaTime);
        }
    }

        private Vector3 GetRandomDestination()
        {
            randomDirection = Random.insideUnitSphere * Mathf.Sqrt(patrolRadiusSquared); // Use square root for radius
            randomDirection += patrolCenter.transform.position;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, Mathf.Sqrt(patrolRadiusSquared), NavMesh.AllAreas); // Use square root for radius
            return hit.position;
        }
    }
}
