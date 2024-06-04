using UnityEngine;
using UnityEngine.AI;

namespace BasicEnemySlime
{
    public class ChasePlayerNode : IBaseNode
    {
        BasicEnemySlime slime;
        NavMeshAgent agent;
        private Vector3 playerPosition;
        private Vector3 lastPosition;

        // Timer
        private float stuckTimeThreshold = 3f;
        private float currentStuckTime = 0f;

        public ChasePlayerNode(BasicEnemySlime slime)
        {
            this.slime = slime;
            this.agent = slime.agent;
            lastPosition = agent.transform.position;
        }

        public virtual bool Update()
        {
            playerPosition = Blackboard.instance.GetPlayerPosition();
            
            // Check if player is within chase range
            if (Vector3.Distance(agent.transform.position, playerPosition) <= slime.chaseRange)
            {
                agent.isStopped = true;
                Vector3 directionToPlayer = (playerPosition - agent.transform.position).normalized;
                Vector3 destinationPoint = playerPosition - directionToPlayer * slime.stopDistance;
                agent.isStopped = false;

                if(!slime.animator.GetBool(slime.animIDAttack))
                {
                    RotateTowardsPlayer(directionToPlayer);
                }
                
                // Check if agent is stuck or standing still
                if (Vector3.Distance(agent.transform.position, lastPosition) < 0.1f)
                {
                    currentStuckTime += Time.deltaTime;
                    if (currentStuckTime >= stuckTimeThreshold)
                    {
                        // Agent is stuck or standing still, recalculate path to player
                        NavMeshPath newPath = new();
                        agent.CalculatePath(playerPosition, newPath);

                        // Check if a valid path exists
                        if (newPath.status != NavMeshPathStatus.PathInvalid && newPath.corners.Length > 1)
                        {
                            slime.animator.SetBool(slime.animIDWalking, true);
                            agent.SetDestination(destinationPoint);
                        }
                        currentStuckTime = 0f;
                        return true;
                    }
                }
                else
                {
                    currentStuckTime = 0f; // Reset stuck time if agent is moving
                }
                return true;
            }
            return false;
        }

        private void RotateTowardsPlayer(Vector3 directionToPlayer)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            float rotationStep = agent.angularSpeed * Time.deltaTime;
            agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, targetRotation, rotationStep);
            lastPosition = agent.transform.position; // Update last position
        }
    }
}
