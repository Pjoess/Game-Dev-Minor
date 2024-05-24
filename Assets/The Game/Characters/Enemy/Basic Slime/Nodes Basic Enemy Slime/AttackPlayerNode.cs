using System.Collections;
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

        private Animator animator;
        private int animIDAnticipate;
        private int animIDAttack;

        public AttackPlayerNode(NavMeshAgent agent, float attackRange, Animator animator, int animIDAnticipate, int animIDAttack)
        {
            this.agent = agent;
            this.attackRange = attackRange;
            this.animator = animator;
            this.animIDAnticipate = animIDAnticipate;
            this.animIDAttack = animIDAttack;
        }

        public virtual bool Update()
        {
            // Update the player's position
            playerPosition = Blackboard.instance.GetPlayerPosition();

            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerPosition);
            damageTimer += Time.deltaTime; // Update damage timer

            if (distanceToPlayer <= attackRange && damageTimer >= damageCooldown)
            {
                agent.isStopped = true;
                // Set animation parameters
                animator.SetBool(animIDAnticipate, true);

                if(BasicEnemySlime.hasAttacked)
                {
                    Blackboard.instance.HitPlayer(5, agent.gameObject.transform.position);
                    damageTimer = 0f;
                }
                return true;
            }
            return false;
        }
    }
}
