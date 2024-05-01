using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class AttackPlayerNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Vector3 playerPosition;
        private float attackRange;
        private float offsetDistance;
        private int damage;
        private LayerMask attackLayer;

        public AttackPlayerNode(NavMeshAgent agent, float attackRange, float offsetDistance, int damage, LayerMask attackLayer)
        {
            this.agent = agent;
            this.attackRange = attackRange;
            this.offsetDistance = offsetDistance;
            this.damage = damage;
            this.attackLayer = attackLayer;
            playerPosition = Blackboard.instance.GetPlayerPosition();
        }

        public virtual bool Update()
        {
            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerPosition);

            if (distanceToPlayer <= attackRange)
            {
                // Check if the player is still within attack range and not behind an obstacle
                if (Physics.Raycast(agent.transform.position, (playerPosition - agent.transform.position).normalized, out RaycastHit hit, distanceToPlayer, attackLayer))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Blackboard.instance.Hit(25);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
