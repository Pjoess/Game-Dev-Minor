using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class FollowNode : IBaseNode
    {
        private NavMeshAgent agent;
        private float maxAgentToPlayerDistance;
        private Animator animator;
        private int animIDWalk;

        public FollowNode(NavMeshAgent agent, float maxAgentToPlayerDistance, Animator animator, int animIDWalk)
        {
            this.agent = agent;
            this.maxAgentToPlayerDistance = maxAgentToPlayerDistance;
            this.animator = animator;
            this.animIDWalk = animIDWalk;
        }

        public virtual bool Update()
        {
            if (agent != null)
            {
                Vector3 playerPosition = Blackboard.instance.GetPlayerPosition();

                if (playerPosition != Vector3.zero)
                {
                    float agentToPlayerDistance = Vector3.Distance(agent.transform.position, playerPosition);

                    if (agentToPlayerDistance > maxAgentToPlayerDistance)
                    {
                        agent.SetDestination(playerPosition);
                        animator.SetBool(animIDWalk, true);
                    }
                    else
                    {
                        // Stop agent when within maxAgentToPlayerDistance
                        agent.ResetPath();
                        animator.SetBool(animIDWalk, false);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
