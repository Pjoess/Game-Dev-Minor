using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class PatrolNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Vector3 playerPosition;
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
            playerPosition = Blackboard.instance.GetPlayerPosition();
            patrolTimer += Time.deltaTime;

            // Check for player presence and chase if within chase range
            if (PlayerWithinChaseRange())
            {
                return false; // Indicate that patrolling should stop
            }
            
            if (Vector3.Distance(agent.transform.position, currentDestination) <= stopDistance || patrolTimer >= patrolInterval)
            {
                currentDestination = GetRandomDestination();
                agent.SetDestination(currentDestination);
                patrolTimer = 0f;
            }

            
            return true;
        }

        private Vector3 GetRandomDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += patrolCenter.transform.position;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas);
            return hit.position;
        }

        private bool PlayerWithinChaseRange()
        {
            if (playerPosition != null)
            {
                float distanceToPlayer = Vector3.Distance(playerPosition, agent.transform.position);
                return distanceToPlayer <= chaseRange;
            }
            return false;
        }
    }
}
