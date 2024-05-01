using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class FollowNode : IBaseNode
    {   
        private NavMeshAgent agent;
        private float maxAgentToPlayerDistance;
        private Vector3 playerPosition;

        public FollowNode(NavMeshAgent agent, float maxAgentToPlayerDistance)
        {
            this.agent = agent;
            this.maxAgentToPlayerDistance = maxAgentToPlayerDistance;
            playerPosition = Blackboard.instance.GetPlayerPosition();
        }

        public virtual bool Update()
        {
            if (agent != null)
            {
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
                    return false;
                }
            }
            return true;
        }
    }
}
