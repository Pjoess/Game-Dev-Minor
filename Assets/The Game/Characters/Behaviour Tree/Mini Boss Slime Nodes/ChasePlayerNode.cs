using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    // Node for chasing the player
    public class ChasePlayerNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Transform playerTransform;
        private float chaseRange;

        public ChasePlayerNode(NavMeshAgent agent, Transform playerTransform, float chaseRange)
        {
            this.agent = agent;
            this.playerTransform = playerTransform;
            this.chaseRange = chaseRange;
        }

        public virtual bool Update()
        {
            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

            if (distanceToPlayer <= chaseRange)
            {
                agent.SetDestination(playerTransform.position);
                return true;
            }
            return false;
        }
    }
}