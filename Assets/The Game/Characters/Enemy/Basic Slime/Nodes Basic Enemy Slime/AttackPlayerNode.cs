using UnityEngine;
using UnityEngine.AI;

namespace BasicEnemySlime
{
    public class AttackPlayerNode : IBaseNode
    {
        BasicEnemySlime slime;
        private NavMeshAgent agent;
        private Vector3 playerPosition;
        private float damageCooldown = 1f;
        private float damageTimer = 0f;

        public AttackPlayerNode(BasicEnemySlime slime)
        {
            this.slime = slime;
            this.agent = slime.agent;

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

            if (distanceToPlayer <= slime.attackRange && damageTimer >= damageCooldown)
            {
                agent.ResetPath();
                // Check if the player is within the cone
                if (angleToPlayer <= slime.coneWidth / 2f && distanceToPlayer <= slime.coneLength)
                {
                    agent.isStopped = true;
                    slime.animator.SetBool(slime.animIDAnticipate, true);

                    if (slime.hasAttacked)
                    {
                        //Blackboard.instance.HitPlayer(10, agent.gameObject.transform.position);
                        damageTimer = 0f;
                        slime.animator.SetBool(slime.animIDAnticipate, false);
                    }
                    return true;
                }

                if (!slime.animator.GetBool(slime.animIDAttack))
                {
                    RotateTowardsPlayer(directionToPlayer);
                }

                return true;
            }
            else if (slime.animator.GetBool(slime.animIDAttack)) return true;
            else return false;
        }

        private void RotateTowardsPlayer(Vector3 directionToPlayer)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            float rotationStep = agent.angularSpeed * Time.deltaTime;
            agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, targetRotation, rotationStep);
        }
    }
}
