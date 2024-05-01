using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class FollowNode : IBaseNode
    {   
        private NavMeshAgent agent;
        private float maxAgentToPlayerDistance;

        public FollowNode(NavMeshAgent agent, float maxAgentToPlayerDistance)
        {
            this.agent = agent;
            this.maxAgentToPlayerDistance = maxAgentToPlayerDistance;
        }

        public virtual bool Update()
        {
            if (agent != null)
            {
                Vector3 playerPosition = Blackboard.instance.GetPlayerPosition();

                if (playerPosition != Vector3.zero)
                {
                    float agentToPlayerDistance = Vector3.Distance(agent.transform.position, playerPosition);

                    if (agentToPlayerDistance >= maxAgentToPlayerDistance)
                    {
                        Debug.Log(maxAgentToPlayerDistance);
                        agent.SetDestination(playerPosition);
                        Debug.Log("Moving");
                        Blackboard.instance.animator.SetBool(Blackboard.instance.animIDWalk, true);
                    }
                    else
                    {
                        Blackboard.instance.animator.SetBool(Blackboard.instance.animIDWalk, false);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
