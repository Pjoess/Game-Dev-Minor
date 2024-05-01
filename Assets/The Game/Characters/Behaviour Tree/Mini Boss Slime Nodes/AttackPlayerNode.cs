using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class AttackPlayerNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Transform playerTransform;
        private float attackRange;
        private float offsetDistance;
        private int damage;
        private LayerMask attackLayer;

        public AttackPlayerNode(NavMeshAgent agent, Transform playerTransform, float attackRange, float offsetDistance, int damage, LayerMask attackLayer)
        {
            this.agent = agent;
            this.playerTransform = playerTransform;
            this.attackRange = attackRange;
            this.offsetDistance = offsetDistance;
            this.damage = damage;
            this.attackLayer = attackLayer;
        }

        public virtual bool Update()
        {
            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

            if (distanceToPlayer <= attackRange)
            {
                // Check if the player is still within attack range and not behind an obstacle
                if (Physics.Raycast(agent.transform.position, (playerTransform.position - agent.transform.position).normalized, out RaycastHit hit, distanceToPlayer, attackLayer))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        playerTransform.GetComponent<Player_Manager>().Hit(damage);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
