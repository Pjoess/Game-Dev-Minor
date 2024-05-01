using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class ChasePlayerNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Vector3 playerPosition;
        private float chaseRange;
        private float stopDistance = 2f; // Distance to stop from player

        public ChasePlayerNode(NavMeshAgent agent, float chaseRange)
        {
            this.agent = agent;
            this.chaseRange = chaseRange;
        }

        public virtual bool Update()
        {
            playerPosition = Blackboard.instance.GetPlayerPosition();
            // Check if player is within chase range
            if (Vector3.Distance(agent.transform.position, playerPosition) <= chaseRange)
            {
                // Calculate direction to player
                Vector3 directionToPlayer = (playerPosition - agent.transform.position).normalized;
                Vector3 destinationPoint = playerPosition - directionToPlayer * stopDistance;

                // Calculate a path to the player's position
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(destinationPoint, path);

                // Check if a valid path exists
                if (path.status != NavMeshPathStatus.PathInvalid)
                {
                    // Set destination to the next corner of the path
                    if (path.corners.Length > 1)
                    {
                        agent.SetDestination(path.corners[1]);
                    }
                    else
                    {
                        agent.SetDestination(destinationPoint);
                    }

                    // Rotate towards the player
                    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                    float rotationStep = agent.angularSpeed * Time.deltaTime;
                    agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, targetRotation, rotationStep);

                    return true;
                }
            }

            return false;
        }
    }
}
