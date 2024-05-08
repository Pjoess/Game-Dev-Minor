using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class ShootMortarNode : IBaseNode
    {
        private NavMeshAgent agent;
        private Transform targetEnemy;
        private LayerMask attackLayer;
        private GameObject mortarPrefab;

        private float shootingRange;
        private float mortarSpawnHeight;
        private float bulletLifetime;
        private bool isShooting = false;

        public Animator animator;
        public int animIDShootingMortar;

        public ShootMortarNode(NavMeshAgent agent, float shootingRange, LayerMask attackLayer,
            float mortarSpawnHeight, GameObject mortarPrefab,
            float bulletLifetime, Animator animator, int animIDShootingMortar)
        {
            this.agent = agent;
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
            if (!isShooting && !Blackboard.instance.IsMortarOnCooldown())
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    targetEnemy = FindClosestEnemy();
                    if (targetEnemy != null && mortarPrefab != null)
                    {
                        isShooting = true;
                        agent.isStopped = true;
                        Blackboard.instance.StartMortarCooldown();
                    }
                }
            }
            else if (isShooting)
            {
                animator.SetBool(animIDShootingMortar, true);
                ShootMortar();
            }
            else
            {
                animator.SetBool(animIDShootingMortar, false);
            }
            return true;
        }

        private void ShootMortar()
        {
            agent.transform.LookAt(targetEnemy);

            Vector3 spawnPosition = targetEnemy.position + Vector3.up * mortarSpawnHeight;
            GameObject mortar = Object.Instantiate(mortarPrefab, spawnPosition, Quaternion.identity);
            Object.Destroy(mortar, bulletLifetime);
            isShooting = false;
            agent.isStopped = false;
        }

        private Transform FindClosestEnemy()
        {
            Collider[] enemies = Physics.OverlapSphere(agent.transform.position, shootingRange, attackLayer);

            Transform closestEnemy = null;
            float closestEnemyDistance = Mathf.Infinity;

            foreach (Collider enemyCollider in enemies)
            {
                if (enemyCollider.CompareTag("Enemy"))
                {
                    Transform enemyTransform = enemyCollider.transform;
                    float distanceToEnemy = Vector3.Distance(agent.transform.position, enemyTransform.position);

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