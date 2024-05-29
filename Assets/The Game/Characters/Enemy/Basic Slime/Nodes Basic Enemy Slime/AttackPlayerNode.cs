using UnityEngine;
using UnityEngine.AI;

namespace BasicEnemySlime
{
    public class AttackPlayerNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Vector3 playerPosition;

        private float attackRange;
        private float damageCooldown = 1f;
        private float damageTimer = 0f;

        private float coneWidth;
        private float coneLength;

        private Animator animator;
        private int animIDAnticipate;
        private int animIDAttack;

        public AttackPlayerNode(NavMeshAgent agent, float attackRange, float coneWidth, float coneLength, Animator animator, int animIDAnticipate, int animIDAttack)
        {
            this.agent = agent;
            this.attackRange = attackRange;
            this.coneWidth = coneWidth;
            this.coneLength = coneLength;
            this.animator = animator;
            this.animIDAnticipate = animIDAnticipate;
            this.animIDAttack = animIDAttack;

            // Disable agent's automatic rotation
            this.agent.updateRotation = false;
        }

        public virtual bool Update()
        {
            playerPosition = Blackboard.instance.GetPlayerPosition();
            Vector3 directionToPlayer = playerPosition - agent.transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;
            float angleToPlayer = Vector3.Angle(agent.transform.forward, directionToPlayer);

            damageTimer += Time.deltaTime;

            if (distanceToPlayer <= attackRange && damageTimer >= damageCooldown)
            {
                // Check if the player is within the cone
                if (angleToPlayer <= coneWidth / 2f && distanceToPlayer <= coneLength)
                {
                    agent.isStopped = true;
                    animator.SetBool(animIDAnticipate, true);

                    if (BasicEnemySlime.hasAttacked)
                    {
                        Blackboard.instance.HitPlayer(10, agent.gameObject.transform.position);
                        damageTimer = 0f;
                        animator.SetBool(animIDAnticipate, false);
                        agent.updateRotation = true;
                        agent.isStopped = false;
                    }
                    return true;
                }
                
                if (!animator.GetBool(animIDAttack))
                {
                    RotateTowardsPlayer(directionToPlayer);
                }
            }
            return false;
        }

        private void RotateTowardsPlayer(Vector3 directionToPlayer)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            float rotationStep = agent.angularSpeed * Time.deltaTime;
            agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, targetRotation, rotationStep);
        }
    }
}
