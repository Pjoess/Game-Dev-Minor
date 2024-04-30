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
                if (Vector3.Distance(agent.transform.position, enemyTransform.position) <= shootingRange)
                {
                    // Calculate the direction to the enemy
                    Vector3 direction = (enemyTransform.position - agent.transform.position).normalized;

                    // Calculate the rotation to face the enemy
                    Quaternion agentRotation = Quaternion.LookRotation(direction);

                    // Rotate the agent to face the enemy
                    agent.transform.rotation = agentRotation;

                    // Calculate the spawn position of the bullet
                    Vector3 bulletSpawnPosition = agent.transform.position + bulletShootHeight * Vector3.up;

                    // Instantiate the bullet prefab with the calculated rotation
                    GameObject bullet = Object.Instantiate(bulletPrefab, bulletSpawnPosition, agentRotation);
                    Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
                    if (bulletRigidbody != null)
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
