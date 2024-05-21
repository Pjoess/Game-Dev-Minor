using UnityEngine;
using UnityEngine.AI;

namespace BasicEnemySlime
{
    public class ReturnBackToPosition : IBaseNode
    {
        private NavMeshAgent agent;
        private Vector3 playerPosition;
        private Vector3 initialPosition;

        private float patrolRadius;
        private float stopDistance;
        private float chaseRange;
        private float attackRange;
        private Animator animator;
        private int animIDWalking;

        public ReturnBackToPosition(NavMeshAgent agent, Vector3 initialPosition, float stopDistance, float chaseRange,
            float attackRange, Animator animator, int animIDWalking)
        {
            this.agent = agent;
            this.initialPosition = initialPosition;    
            this.stopDistance = stopDistance;
            this.chaseRange = chaseRange;
            this.attackRange = attackRange;
            this.animator = animator;
            this.animIDWalking = animIDWalking;
        }

        public virtual bool Update()
        {
            playerPosition = Blackboard.instance.GetPlayerPosition();

            if (agent.velocity.magnitude <= 0.01f)
            {
                animator.SetBool(animIDWalking, false);
                return false;
            }

            if (Vector3.Distance(agent.transform.position, playerPosition) <= chaseRange)
            {
                animator.SetBool(animIDWalking, true);
                return false;
            }
            
            if (Vector3.Distance(agent.transform.position, playerPosition) <= stopDistance)
            {
                animator.SetBool(animIDWalking, false);
                return false;
            }

            if (Vector3.Distance(agent.transform.position, playerPosition) > chaseRange)
            {
                animator.SetBool(animIDWalking, true);
                agent.SetDestination(initialPosition);
                return false;
            }
            return true;
        }
    }
}