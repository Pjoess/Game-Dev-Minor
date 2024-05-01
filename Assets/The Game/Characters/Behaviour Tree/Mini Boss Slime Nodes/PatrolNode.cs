using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class PatrolNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Vector3 playerPosition;
        private float chaseRange;
        private float stopDistance = 4f;

        public virtual bool Update()
        {
            playerPosition = Blackboard.instance.GetPlayerPosition();
            
            return true;
        }
    }
}
