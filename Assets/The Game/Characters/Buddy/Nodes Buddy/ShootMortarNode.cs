using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class ShootMortarNode : IBaseNode
    {
        Buddy_Agent agent;
        private NavMeshAgent navAgent;
        private Transform targetEnemy;
        private LayerMask attackLayer;
        private GameObject mortarPrefab;

        private float shootingRange;
        private float mortarSpawnHeight;
        private float bulletLifetime;

        public Animator animator;
        public int animIDShootingMortar;

        public ShootMortarNode(Buddy_Agent agent, NavMeshAgent navAgent, float shootingRange, LayerMask attackLayer,
            float mortarSpawnHeight, GameObject mortarPrefab,
            float bulletLifetime, Animator animator, int animIDShootingMortar)
        {
            this.agent = agent;
            this.navAgent = navAgent;
            this.shootingRange = shootingRange;
            this.attackLayer = attackLayer;
            this.mortarPrefab = mortarPrefab;
            this.mortarSpawnHeight = mortarSpawnHeight;
            this.bulletLifetime = bulletLifetime; // Set the bullet lifetime
            this.animator = animator;
            this.animIDShootingMortar = animIDShootingMortar;
        }

        public bool Update()
        {
            if (Blackboard.instance.IsMortarReady())
            {
                targetEnemy = FindClosestEnemy();
                if (targetEnemy != null && mortarPrefab != null)
                {
                    agent.canShootMortar = true;
                    if (agent.shootMortar)
                    {
                        navAgent.isStopped = true;
                        animator.SetBool(animIDShootingMortar, true);
                        ShootMortar();
                        Blackboard.instance.ResetMortar();
                    }
                }
                else agent.canShootMortar = false;
            }
            else
            {
                agent.canShootMortar = false;
                animator.SetBool(animIDShootingMortar, false);
            }
                
            return true;
        }

        private void ShootMortar()
        {
            navAgent.transform.LookAt(targetEnemy);

            //Vector3 spawnPosition = targetEnemy.position + Vector3.up * mortarSpawnHeight;
            GameObject mortar = Object.Instantiate(mortarPrefab, targetEnemy.position, Quaternion.identity);
            //Object.Destroy(mortar, bulletLifetime);
            navAgent.isStopped = false;
            agent.shootMortar = false;
            //animator.SetBool(animIDShootingMortar, false);
        }

        private Transform FindClosestEnemy()
        {
            Collider[] enemies = Physics.OverlapSphere(navAgent.transform.position, shootingRange, attackLayer);

            Transform closestEnemy = null;
            float closestEnemyDistance = Mathf.Infinity;

            foreach (Collider enemyCollider in enemies)
            {
                if (enemyCollider.CompareTag("Enemy"))
                {
                    Transform enemyTransform = enemyCollider.transform;
                    float distanceToEnemy = Vector3.Distance(navAgent.transform.position, enemyTransform.position);

                    if (distanceToEnemy < closestEnemyDistance)
                    {
                        closestEnemy = enemyTransform;
                        closestEnemyDistance = distanceToEnemy;
                    }
                }
            }
            return closestEnemy;
        }
    }
}