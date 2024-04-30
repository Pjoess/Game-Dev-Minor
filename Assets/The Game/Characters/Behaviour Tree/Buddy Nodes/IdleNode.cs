using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class IdleNode : IBaseNode
    {
        private NavMeshAgent agent;

        public IdleNode(NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public virtual bool Update()
        {
            if (agent.velocity.magnitude < 1f)
            {
                Debug.Log("Idle");
                return true;
            }
            return false;
        }
    }
}
