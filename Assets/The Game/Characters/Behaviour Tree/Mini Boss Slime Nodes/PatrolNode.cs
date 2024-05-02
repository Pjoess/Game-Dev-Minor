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
        private Vector3 currentDestination;
        private float patrolTimer = 0f;
        private float patrolInterval = 2f;
        private float chaseRange;

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
            // Check if the player is within the chase range
            if (Vector3.Distance(agent.transform.position, Blackboard.instance.GetPlayerPosition()) > chaseRange)
            {
                // Go back to the patrol center and patrol around it
                currentDestination = patrolCenter.transform.position;
                agent.SetDestination(currentDestination);
                patrolTimer = 0f; // Reset patrol timer

                // Increment patrol timer
                patrolTimer += Time.deltaTime;

                // Check if reached current destination or patrol interval is reached
                if (Vector3.Distance(agent.transform.position, currentDestination) <= stopDistance || patrolTimer >= patrolInterval)
                {
                    // Get a new random destination
                    currentDestination = GetRandomDestination();
                    agent.SetDestination(currentDestination);
                    patrolTimer = 0f; // Reset patrol timer
                }
                return true; // Return true as the agent is going back to the patrol center
            }
            else if (Vector3.Distance(agent.transform.position, Blackboard.instance.GetPlayerPosition()) < chaseRange)
            {
                return false; // Return true as the agent is going back to the patrol center
            }

            return true; // Return false as the patrol behavior continues
        }

        // Get a random destination within patrol radius from patrol center
        private Vector3 GetRandomDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += patrolCenter.transform.position;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas);
            return hit.position;
        }
    }
}
