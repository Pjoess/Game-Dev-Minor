using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class AttackPlayerOnPositionNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Vector3 playerPosition;
        private float attackRange;
        private float offsetDistance;
        private LayerMask attackLayer;
        private float coneWidth;
        private float coneLength;

        private Animator animator;
        private int animIDAnticipate;
        private int animIDAttack;

        public AttackPlayerOnPositionNode(NavMeshAgent agent, float attackRange, 
            float offsetDistance, LayerMask attackLayer, float coneWidth, 
            float coneLength, Animator animator, int animIDAnticipate, int animIDAttack)
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

            // Disable agent's automatic rotation
            this.agent.updateRotation = false;
        }

        public virtual bool Update()
        {
            // Update the player's position
            playerPosition = Blackboard.instance.GetPlayerPosition();
            Vector3 directionToPlayer = playerPosition - agent.transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            // Check if the player is within attack range
            if (distanceToPlayer <= attackRange)
            {
                if (IsPlayerWithinCone(directionToPlayer))
                {
                    return true;
                }
                else
                {
                    if(animator.GetBool(animIDAttack) == false)
                    {
                        // Smoothly rotate towards the player
                        RotateTowardsPlayer(directionToPlayer);
                    }
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
                return true;
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
