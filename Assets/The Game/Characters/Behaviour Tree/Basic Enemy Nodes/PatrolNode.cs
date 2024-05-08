using UnityEngine;
using UnityEngine.AI;

namespace BasicEnemySlime
{
    public class PatrolNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Vector3 playerPosition;
        private GameObject patrolCenter;

        private float patrolRadius;
        private float stopDistance;
        private float chaseRange;
        private float attackRange;

        private Vector3 currentDestination;
        private float patrolTimer = 0f;
        private float patrolInterval = 2f;

        private float waitTimer = 0f;
        private float waitDuration = 6f; // Wait duration in seconds

        private Animator animator;
        private int animIDWalking;

        public PatrolNode(NavMeshAgent agent, GameObject patrolCenter, float patrolRadius, float stopDistance, float chaseRange,
            float attackRange, Animator animator, int animIDWalking)
        {
            this.agent = agent;
            this.patrolCenter = patrolCenter;
            this.patrolRadius = patrolRadius;
            this.stopDistance = stopDistance;
            this.chaseRange = chaseRange;
            this.attackRange = attackRange;
            this.animator = animator;
            this.animIDWalking = animIDWalking;
            currentDestination = GetRandomDestination();
        }

        public virtual bool Update()
        {
            playerPosition = Blackboard.instance.GetPlayerPosition();
            patrolTimer += Time.deltaTime;

            if (Vector3.Distance(playerPosition, agent.transform.position) <= chaseRange)
            {
                return false;
            }
            
            if (Vector3.Distance(agent.transform.position, currentDestination) <= stopDistance)
            {
                // Agent has reached its destination
                animator.SetBool(animIDWalking, false); // Stop walking animation
                waitTimer += Time.deltaTime;

                if (waitTimer >= waitDuration)
                {
                    // Reset wait timer and get a new destination
                    waitTimer = 0f;
                    currentDestination = GetRandomDestination();
                    animator.SetBool(animIDWalking, true);
                    agent.SetDestination(currentDestination);
                }
            }
            else if (patrolTimer >= patrolInterval)
            {
                // Time to change destination
                currentDestination = GetRandomDestination();
                animator.SetBool(animIDWalking, true);
                agent.SetDestination(currentDestination);
                patrolTimer = 0f;
            }           
            return true;
        }

        private Vector3 GetRandomDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += patrolCenter.transform.position;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas);
            return hit.position;
        }
    }
}
