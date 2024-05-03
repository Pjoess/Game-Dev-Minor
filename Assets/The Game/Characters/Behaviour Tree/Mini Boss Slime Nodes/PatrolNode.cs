using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class PatrolNode : IBaseNode
    {
        private NavMeshAgent agent;
        private GameObject patrolCenter;
        private float patrolRadius;
        private float stopDistance;
        private float chaseRange;
        private Vector3 currentDestination;
        private float patrolTimer = 0f;
        private float patrolInterval = 2f;

        public PatrolNode(NavMeshAgent agent, GameObject patrolCenter, float patrolRadius, float stopDistance, float chaseRange)
        {
            this.agent = agent;
            this.patrolCenter = patrolCenter;
            this.patrolRadius = patrolRadius;
            this.stopDistance = stopDistance;
            this.chaseRange = chaseRange;
            currentDestination = GetRandomDestination();
        }

        public virtual bool Update()
        {
            patrolTimer += Time.deltaTime;
            if (Vector3.Distance(agent.transform.position, currentDestination) <= stopDistance || patrolTimer >= patrolInterval)
            {
                currentDestination = GetRandomDestination();
                agent.SetDestination(currentDestination);
                patrolTimer = 0f;
            }

            // Check for player presence and chase if within chase range
            if (PlayerWithinChaseRange())
            {
                return false; // Indicate that patrolling should stop
            }

            // No need to rotate if the agent has reached its destination
            if (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
            {
                RotateTowardsDestination();
            }
            
            return true; // Continue patrolling
        }

#pragma warning disable // Hides The Warnings Temporary (asserion failed) does not affect the game
        private void RotateTowardsDestination()
        {
            Vector3 directionToDestination = currentDestination - agent.transform.position;
            if (directionToDestination.magnitude > Mathf.Epsilon) // Check if the magnitude is greater than epsilon
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToDestination);
                agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, lookRotation, agent.angularSpeed * Time.deltaTime);
            }
        }
#pragma warning restore

        private Vector3 GetRandomDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += patrolCenter.transform.position;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas);
            return hit.position;
        }

        private bool PlayerWithinChaseRange()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(player.transform.position, agent.transform.position);
                return distanceToPlayer <= chaseRange;
            }
            return false;
        }
    }
}
