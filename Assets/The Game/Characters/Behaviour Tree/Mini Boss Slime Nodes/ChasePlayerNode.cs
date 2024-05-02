using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class ChasePlayerNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Vector3 playerPosition;
        private float chaseRange;
        private float stopDistance = 2f;
        private Vector3 lastPosition;
        private float stuckTimeThreshold = 3f;
        private float currentStuckTime = 0f;

        public ChasePlayerNode(NavMeshAgent agent, float chaseRange)
        {
            this.agent = agent;
            this.chaseRange = chaseRange;
            lastPosition = agent.transform.position;
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

                // Check if agent is stuck or standing still
                if (Vector3.Distance(agent.transform.position, lastPosition) < 0.1f)
                {
                    currentStuckTime += Time.deltaTime;
                    if (currentStuckTime >= stuckTimeThreshold)
                    {
                        // Agent is stuck or standing still, recalculate path to player
                        NavMeshPath newPath = new NavMeshPath();
                        agent.CalculatePath(playerPosition, newPath);

                        // Check if a valid path exists
                        if (newPath.status != NavMeshPathStatus.PathInvalid && newPath.corners.Length > 1)
                        {
                            // Set the new destination
                            agent.SetDestination(newPath.corners[newPath.corners.Length - 1]);
                        }

                        currentStuckTime = 0f;
                    }
                }
                else
                {
                    currentStuckTime = 0f; // Reset stuck time if agent is moving
                }

                // Rotate towards the player
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                float rotationStep = agent.angularSpeed * Time.deltaTime;
                agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, targetRotation, rotationStep);

                lastPosition = agent.transform.position; // Update last position

                return true;
            }

            return false;
        }
    }
}
