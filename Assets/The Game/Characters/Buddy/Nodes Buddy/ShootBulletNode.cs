using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class ShootBulletNode : IBaseNode
    {
        private Buddy_Agent buddy;
        private NavMeshAgent agent;
        private float shootingRange;
        private LayerMask attackLayer;
        private float bulletShootHeight;
        private float bulletSpeed;
        private float bulletLifetime;
        private GameObject bulletPrefab;
        private float shootTimer = 0f;
        private const float timeBetweenShots = 2f; // Time between each shot
        private Animator animator;
        private int animIDShooting;
        private AudioSource shootingSound;

        public ShootBulletNode(Buddy_Agent buddy)
        {
            this.buddy = buddy;
            agent = buddy.agent;
            shootingRange = buddy.shootingRange;
            attackLayer = buddy.attackLayer;
            bulletShootHeight = buddy.bulletShootHeight;
            bulletSpeed = buddy.bulletSpeed;
            bulletLifetime = buddy.bulletLifetime;
            bulletPrefab = buddy.bulletPrefab;
            animator = buddy.animator;
            animIDShooting = buddy.animIDShooting;
            shootingSound = buddy.shootSound;
        }

        public bool Update()
        {
            if (buddy.targetedEnemy != null)
            {
                if (shootTimer >= timeBetweenShots)
                {
                    ShootAtEnemy(buddy.targetedEnemy.transform);
                    animator.SetBool(animIDShooting, true);
                    shootTimer = 0f; // Reset the timer
                }
                else
                {
                    animator.SetBool(animIDShooting, false);
                    shootTimer += Time.deltaTime; // Increment the timer
                }
                return true;
            }
            return false;
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
                    shootingSound.Play();
                    
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
