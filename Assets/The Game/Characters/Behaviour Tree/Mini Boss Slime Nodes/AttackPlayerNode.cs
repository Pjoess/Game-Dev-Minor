using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SlimeMiniBoss
{
    public class AttackPlayerNode : IBaseNode
    {
        private NavMeshAgent agent;
        private float attackRange;
        private float offsetDistance; // Added offsetDistance parameter
        private LayerMask attackLayer;
        private float coneWidth;
        private float coneLength;
        private float damageTimer = 0f;
        private float damageCooldown = 3f;

        public AttackPlayerNode(NavMeshAgent agent, float attackRange, float offsetDistance, LayerMask attackLayer, float coneWidth, float coneLength)
        {
            this.agent = agent;
            this.attackRange = attackRange;
            this.offsetDistance = offsetDistance;
            this.attackLayer = attackLayer;
            this.coneWidth = coneWidth;
            this.coneLength = coneLength;
        }

        public virtual bool Update()
        {
            // Get the player's position
            Vector3 playerPosition = Blackboard.instance.GetPlayerPosition();

            // Calculate distance to the player
            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerPosition);

            // Update damage timer
            damageTimer += Time.deltaTime;

            // Check if the player is within attack range and the damage cooldown has passed
            if (distanceToPlayer <= attackRange && damageTimer >= damageCooldown)
            {
                // Check if the player is within the cone
                if (IsPlayerWithinCone(agent.transform.forward, coneWidth, coneLength))
                {
                    // Apply damage to the player
                    Blackboard.instance.Hit(20);

                    // Reset damage timer
                    damageTimer = 0f;

                    return true;
                }
            }

            return false;
        }

        // Method to check if the player is within the cone
        private bool IsPlayerWithinCone(Vector3 direction, float coneWidth, float coneLength)
        {
            // Get the player's position
            Vector3 playerPosition = Blackboard.instance.GetPlayerPosition();

            // Calculate the direction to the player
            Vector3 directionToPlayer = playerPosition - agent.transform.position;

            // Calculate the angle between the direction the agent is facing and the direction to the player
            float angleToPlayer = Vector3.Angle(direction, directionToPlayer);

            // Check if the player is within the cone width and cone length
            if (angleToPlayer <= coneWidth / 2f && directionToPlayer.magnitude <= coneLength)
            {
                Debug.Log("Player within cone!");
                Blackboard.instance.Hit(10);
                return true;
            }

            return false;
        }

    }
}
