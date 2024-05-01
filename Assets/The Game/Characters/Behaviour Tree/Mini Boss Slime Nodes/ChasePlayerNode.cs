using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    // Node for chasing the player
    public class ChasePlayerNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Vector3 playerPosition;
        private float chaseRange;
        private float stopDistance = 4f; // Distance to stop from player

        public ChasePlayerNode(NavMeshAgent agent, float chaseRange)
        {
            this.agent = agent;
            this.chaseRange = chaseRange;
        }

        public virtual bool Update()
        {
            // Get the player's position
            playerPosition = Blackboard.instance.GetPlayerPosition();

            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerPosition);

            if (distanceToPlayer <= chaseRange)
            {
                // Calculate the direction from the boss to the player
                Vector3 directionToPlayer = (playerPosition - agent.transform.position).normalized;
                Vector3 destinationPoint = playerPosition - directionToPlayer * stopDistance;

                agent.SetDestination(destinationPoint);

                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                float rotationStep = agent.angularSpeed * Time.deltaTime;
                agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, targetRotation, rotationStep);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
