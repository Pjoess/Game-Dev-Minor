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
        private Vector3 lastPosition; // Store the last position of the agent
        private float stuckTimeThreshold = 3f; // Time threshold to consider the agent stuck
        private float currentStuckTime = 0f; // Time the agent has been stuck

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

                // Calculate a path to the player's position
                NavMeshPath path = new();
                agent.CalculatePath(destinationPoint, path);

                // Check if a valid path exists
                if (path.status != NavMeshPathStatus.PathInvalid && path.corners.Length > 1)
                {
                    // Check for obstacles along the path and adjust destination
                    int cornerIndex = 1;
                    while (cornerIndex < path.corners.Length)
                    {
                        Vector3 corner = path.corners[cornerIndex];
                        if (Physics.Raycast(agent.transform.position, corner - agent.transform.position, out RaycastHit hit, Vector3.Distance(agent.transform.position, corner)))
                        {
                            // If there's an obstacle, move the destination point to the hit point
                            destinationPoint = hit.point;
                            break;
                        }
                        cornerIndex++;
                    }

                    agent.SetDestination(destinationPoint);

                    // Rotate towards the player
                    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                    float rotationStep = agent.angularSpeed * Time.deltaTime;
                    agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, targetRotation, rotationStep);

                    // Check if agent is stuck
                    if (Vector3.Distance(agent.transform.position, lastPosition) < 0.1f)
                    {
                        currentStuckTime += Time.deltaTime;
                        if (currentStuckTime >= stuckTimeThreshold)
                        {
                            // Agent is stuck, find a new random destination
                            Vector3 randomDirection = Random.insideUnitSphere * 5f; // Adjust 5f according to your environment
                            randomDirection += agent.transform.position;
                            NavMeshHit hit;
                            NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas);
                            agent.SetDestination(hit.position);
                            currentStuckTime = 0f;
                        }
                    }
                    else
                    {
                        currentStuckTime = 0f; // Reset stuck time if agent is moving
                    }

                    lastPosition = agent.transform.position; // Update last position

                    return true;
                }
            }

            return false;
        }
    }
}
