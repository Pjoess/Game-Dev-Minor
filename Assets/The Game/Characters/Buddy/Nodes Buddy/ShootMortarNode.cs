using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class ShootMortarNode : IBaseNode
    {
        Buddy_Agent buddy;
        private NavMeshAgent navAgent;
        private Transform targetEnemy;
        private LayerMask attackLayer;
        private GameObject mortarPrefab;
        private float shootingRange;
        private float mortarSpawnHeight;
        private float bulletLifetime;
        private Animator animator;
        private int animIDShootingMortar;
        private AudioSource shootSound;

        public ShootMortarNode(Buddy_Agent buddy)
        {
            this.buddy = buddy;
            navAgent = buddy.agent;
            shootingRange = buddy.shootingRange;
            attackLayer = buddy.attackLayer;
            mortarPrefab = buddy.mortarPrefab;
            mortarSpawnHeight = buddy.mortarSpawnHeight;
            bulletLifetime = buddy.bulletLifetime; // Set the bullet lifetime
            animator = buddy.animator;
            animIDShootingMortar = buddy.animIDShootingMortar;
            shootSound = buddy.shootSound;
        }

        public bool Update()
        {
            if (Blackboard.instance.IsMortarReady())
            {
                targetEnemy = FindClosestEnemy();
                if (targetEnemy != null && mortarPrefab != null)
                {
                    buddy.canShootMortar = true;
                    if (buddy.shootMortar)
                    {
                        navAgent.isStopped = true;
                        animator.SetBool(animIDShootingMortar, true);
                        ShootMortar();
                        Blackboard.instance.ResetMortar();
                    }
                }
                else buddy.canShootMortar = false;
            }
            else
            {
                buddy.canShootMortar = false;
                animator.SetBool(animIDShootingMortar, false);
            }   
            return true;
        }

        private void ShootMortar()
        {
            navAgent.transform.LookAt(targetEnemy);

            //Vector3 spawnPosition = targetEnemy.position + Vector3.up * mortarSpawnHeight;
            Vector3 spawnPosition = targetEnemy.position;
            spawnPosition.y = buddy.transform.position.y;
            GameObject mortar = Object.Instantiate(mortarPrefab, spawnPosition, Quaternion.identity);
            shootSound.Play();
            //Object.Destroy(mortar, bulletLifetime);
            navAgent.isStopped = false;
            buddy.shootMortar = false;
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