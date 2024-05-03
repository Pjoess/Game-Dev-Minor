using System.Collections;
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
        private LayerMask attackLayer;
        private float coneWidth;
        private float coneLength;
        private float damageTimer = 0f;
        private float damageCooldown = 1f;
        private Vector3 directionToPlayer;

        private Animator animator;
        private int animIDAnticipate;
        private int animIDAttack;

        public AttackPlayerNode(NavMeshAgent agent, float attackRange, float offsetDistance, LayerMask attackLayer, float coneWidth, float coneLength, Animator animator, int animIDAnticipate, int animIDAttack)
        {
            this.agent = agent;
            this.attackRange = attackRange;
            this.offsetDistance = offsetDistance;
            this.attackLayer = attackLayer;
            this.coneWidth = coneWidth;
            this.coneLength = coneLength;
            this.animator = animator;
            this.animIDAnticipate = animIDAnticipate;
            this.animIDAttack = animIDAttack;
        }

        public virtual bool Update()
        {
            // Update the player's position
            playerPosition = Blackboard.instance.GetPlayerPosition();

            directionToPlayer = playerPosition - agent.transform.position;
            float distanceToPlayer = directionToPlayer.magnitude; // Use magnitude for comparison
            damageTimer += Time.deltaTime; // Update damage timer

            // Check if the player is within attack range and the damage cooldown has passed
            if (distanceToPlayer <= attackRange && damageTimer >= damageCooldown)
            {
                if (IsPlayerWithinCone(directionToPlayer))
                {
                    damageTimer = 0f; // Reset damage timer
                    return true;
                }
            }
            return false;
        }

        // Method to check if the player is within the cone
        private bool IsPlayerWithinCone(Vector3 directionToPlayer)
        {
            float angleToPlayer = Vector3.Angle(agent.transform.forward, directionToPlayer);

            // Check if the player is within the cone width and cone length
            if (angleToPlayer <= coneWidth / 2f && directionToPlayer.magnitude <= coneLength)
            {
                animator.SetBool(animIDAnticipate, true);
                animator.SetBool(animIDAttack, true);

                // Check if animation has ended and condition is false (ONLY TEMPORARY Shockwave needs to be fixed)
                if (!animator.IsInTransition(0) && !animator.GetCurrentAnimatorStateInfo(0).IsTag("isIdling") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("isAnticipating"))
                {
                    Blackboard.instance.Hit(10);
                }
                return true;
            }
            return false;
        }
    }
}
