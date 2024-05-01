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
        private TMP_Text buddyCooldownText;

        private float shootingRange;
        private float mortarCooldownTime;
        private float mortarSpawnHeight;
        private float bulletLifetime;
        private float nextMortarTime = 0f;
        private bool isShooting = false;

        public Animator animator;
        public int animIDShootingMortar;
        
        public ShootMortarNode(NavMeshAgent agent, float shootingRange, LayerMask attackLayer, 
            float mortarSpawnHeight, GameObject mortarPrefab, TMP_Text buddyCooldownText, float mortarCooldownTime,
            Animator animator,int animIDShootingMortar)
        {
            this.agent = agent;
            this.shootingRange = shootingRange;
            this.attackLayer = attackLayer;
            this.mortarPrefab = mortarPrefab;
            this.buddyCooldownText = buddyCooldownText;
            this.mortarCooldownTime = mortarCooldownTime;
            this.mortarSpawnHeight = mortarSpawnHeight;
            this.bulletLifetime = 5f; // Set the default bullet lifetime
            this.animator = animator;
            this.animIDShootingMortar = animIDShootingMortar;
        }

        public bool Update()
        {
            if (!isShooting && Time.time >= nextMortarTime)
            {
                buddyCooldownText.text = "Mortar Ready! - (Press B)";

                if (Input.GetKeyDown(KeyCode.B))
                {
                    targetEnemy = FindClosestEnemy();
                    if (targetEnemy != null && mortarPrefab != null)
                    {
                        isShooting = true;
                        agent.isStopped = true;
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
                buddyCooldownText.text = "Cooldown: " + Mathf.CeilToInt(nextMortarTime - Time.time) + "s";
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
            nextMortarTime = Time.time + mortarCooldownTime;
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
