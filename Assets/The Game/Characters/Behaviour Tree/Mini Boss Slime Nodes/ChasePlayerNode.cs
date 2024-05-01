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
            playerPosition = Blackboard.instance.GetPlayerPosition();
        }

        public virtual bool Update()
        {
            // Update the player's position
            playerPosition = Blackboard.instance.GetPlayerPosition();

            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerPosition);

            if (distanceToPlayer <= chaseRange)
            {
                // Calculate the direction from the boss to the player
                Vector3 directionToPlayer = (playerPosition - agent.transform.position).normalized;

                // Calculate the destination point by subtracting the direction multiplied by the stop distance
                Vector3 destinationPoint = playerPosition - directionToPlayer * stopDistance;

                // Set the destination for the boss
                agent.SetDestination(destinationPoint);

                return true;
            }
            else
            {
                // If player is out of range, set destination again until it reaches stop distance from player
                agent.SetDestination(playerPosition);
                return false;
            }
        }
    }
}
