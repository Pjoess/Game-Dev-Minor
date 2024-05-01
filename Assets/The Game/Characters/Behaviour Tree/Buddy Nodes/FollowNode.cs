using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class FollowNode : IBaseNode
    {   
        private NavMeshAgent agent;
        private float playerOutOfRadius;

        public FollowNode(NavMeshAgent agent, float playerOutOfRadius)
        {
            this.agent = agent;
            this.playerOutOfRadius = playerOutOfRadius;
        }

        public virtual bool Update()
        {
            if (agent != null)
            {
                Vector3 playerPosition = Blackboard.instance.GetPlayerPosition();

                if (playerPosition != Vector3.zero)
                {
                    float distanceToPlayer = Vector3.Distance(agent.transform.position, playerPosition);

                    if (distanceToPlayer >= playerOutOfRadius)
                    {
                        Debug.Log(playerOutOfRadius);
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
