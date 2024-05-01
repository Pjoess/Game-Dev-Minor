using System.Collections;
using buddy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class ShootBulletNode : IBaseNode
    {
        private NavMeshAgent agent;
        private float shootingRange;
        private LayerMask attackLayer;
        private float bulletShootHeight;
        private float bulletSpeed;
        private float bulletLifetime;

        private GameObject bulletPrefab;
        
        private float shootTimer = 0f;
        private const float timeBetweenShots = 1f; // Time between each shot

        public ShootBulletNode(NavMeshAgent agent, float shootingRange, LayerMask attackLayer, float bulletShootHeight, float bulletSpeed, float bulletLifetime, GameObject bulletPrefab)
        {
            this.agent = agent;
            this.shootingRange = shootingRange;
            this.attackLayer = attackLayer;
            this.bulletShootHeight = bulletShootHeight;
            this.bulletSpeed = bulletSpeed;
            this.bulletLifetime = bulletLifetime;
            this.bulletPrefab = bulletPrefab;
        }

        public bool Update()
        {
            // Find the closest enemy
            Transform enemyTransform = FindClosestEnemy();

            if (enemyTransform != null)
            {
                if (shootTimer >= timeBetweenShots)
                {
                    ShootAtEnemy(enemyTransform);
                    shootTimer = 0f; // Reset the timer
                }
                else
                {
                    shootTimer += Time.deltaTime; // Increment the timer
                }
            }
            return true;
        }

        Transform FindClosestEnemy()
        {
            Transform closestEnemy = null;
            float closestEnemyDistance = Mathf.Infinity;

            Collider[] enemiesInRange = Physics.OverlapSphere(agent.transform.position, shootingRange, attackLayer);
            
            foreach (Collider enemyCollider in enemiesInRange)
            {
                // Check if the collider belongs to an enemy
                if (enemyCollider.CompareTag("Enemy"))
                {
                    Transform enemyTransform = enemyCollider.transform;
                    
                    // Check if the enemy is within shooting range
                    float distanceToEnemy = Vector3.Distance(agent.transform.position, enemyTransform.position);
                    if (distanceToEnemy <= shootingRange)
                    {
                        if (Physics.Raycast(agent.transform.position, (enemyTransform.position - agent.transform.position).normalized, out RaycastHit hit, distanceToEnemy, attackLayer))
                        {
                            if (hit.collider.CompareTag("Enemy") && distanceToEnemy < closestEnemyDistance)
                            {
                                closestEnemy = enemyTransform;
                                closestEnemyDistance = distanceToEnemy;
                            }
                        }
                    }
                }
            }
            return closestEnemy;
        }

        void ShootAtEnemy(Transform enemyTransform)
        {
            if (bulletPrefab != null)
            {
                if (Vector3.Distance(agent.transform.position, enemyTransform.position) <= shootingRange)
                {
                    Vector3 direction = (enemyTransform.position - agent.transform.position).normalized;
                    Quaternion agentRotation = Quaternion.LookRotation(direction);
                    agent.transform.rotation = agentRotation;
                    Vector3 bulletSpawnPosition = agent.transform.position + bulletShootHeight * Vector3.up;

                    // Instantiate the bullet prefab with the calculated rotation
                    GameObject bullet = Object.Instantiate(bulletPrefab, bulletSpawnPosition, agentRotation);
                    if (bullet.TryGetComponent<Rigidbody>(out var bulletRigidbody))
                    {
                        bulletRigidbody.velocity = direction * bulletSpeed;
                    }
                    Object.Destroy(bullet, bulletLifetime);
                }
            }
            else
            {
                Debug.LogError("Bullet prefab reference is null.");
            }
        }
    }
}
