using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class FollowNode : IBaseNode
    {   
        private NavMeshAgent agent;

        public FollowNode(NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public virtual bool Update()
        {
            if(agent != null)
            {
                if (RandomPoint(Blackboard.instance.GetPlayerPosition(), 5f, out Vector3 randomPoint))
                {
                    agent.SetDestination(randomPoint);
                    if(agent.remainingDistance > 0.5f){
                        Debug.Log("Moving");
                        Blackboard.instance.animator.SetBool(Blackboard.instance.animIDWalk, true);
                    }
                }
                return true;
            }
            return false;
        }

        bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, range, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
            result = Vector3.zero;
            return false;
        }
    }
}
