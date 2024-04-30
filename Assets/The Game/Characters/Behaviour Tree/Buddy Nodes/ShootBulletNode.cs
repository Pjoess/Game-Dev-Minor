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
        private const float timeBetweenShots = 0.5f; // Time between each shot

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
                        // Perform a raycast to check if there are any obstacles between the agent and the enemy
                        RaycastHit hit;
                        if (Physics.Raycast(agent.transform.position, (enemyTransform.position - agent.transform.position).normalized, out hit, distanceToEnemy, attackLayer))
                        {
                            // If the raycast hits an enemy, update closestEnemy and closestEnemyDistance
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
                Debug.Log("Shot");
                if (Vector3.Distance(agent.transform.position, enemyTransform.position) <= shootingRange)
                {
                    agent.transform.LookAt(enemyTransform);

                    Vector3 direction = (enemyTransform.position - agent.transform.position).normalized;
                    Vector3 bulletSpawnPosition = agent.transform.position + bulletShootHeight * Vector3.up;

                    // Instantiate the bullet prefab and set its velocity
                    GameObject bullet = Object.Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
                    if (bullet.TryGetComponent<Rigidbody>(out var bulletRigidbody))
                    {
                        bulletRigidbody.velocity = direction * bulletSpeed;
                    }

                    // Destroy the bullet after its lifetime
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
