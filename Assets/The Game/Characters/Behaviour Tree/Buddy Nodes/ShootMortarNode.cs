using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace buddy
{
    public class ShootMortarNode : IBaseNode
    {
        private NavMeshAgent agent;
        private float shootingRange;
        private LayerMask attackLayer;
        private GameObject mortarPrefab;
        private TMP_Text buddyCooldownText;
        private float mortarCooldownTime;
        private float mortarSpawnHeight;
        private float bulletLifetime;
        private float distanceToMove;
        private float mortarSpeed;
        private float nextMortarTime = 0f;
        private bool isShooting = false;
        private Transform targetEnemy;

        public ShootMortarNode(NavMeshAgent agent, float shootingRange, LayerMask attackLayer, float mortarSpeed, float mortarSpawnHeight, GameObject mortarPrefab, TMP_Text buddyCooldownText, float mortarCooldownTime)
        {
            this.agent = agent;
            this.shootingRange = shootingRange;
            this.attackLayer = attackLayer;
            this.mortarPrefab = mortarPrefab;
            this.buddyCooldownText = buddyCooldownText;
            this.mortarCooldownTime = mortarCooldownTime;
            this.mortarSpeed = mortarSpeed;
            this.mortarSpawnHeight = mortarSpawnHeight;
            this.bulletLifetime = 5f; // Set the default bullet lifetime
            this.distanceToMove = 5f; // Set the default distance to move
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
                ShootMortar();
            }
            else
            {
                buddyCooldownText.text = "Cooldown: " + Mathf.CeilToInt(nextMortarTime - Time.time) + "s";
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

            // Calculate mortar movement over time
            MoveBulletOverTime(mortar);
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

        private IEnumerator MoveBulletOverTime(GameObject mortar)
        {
            Vector3 initialPosition = mortar.transform.position;
            Vector3 targetPosition = initialPosition - Vector3.up * distanceToMove;

            float startTime = Time.time;
            float journeyLength = Vector3.Distance(initialPosition, targetPosition);

            while (mortar != null && (Time.time - startTime) * mortarSpeed < journeyLength)
            {
                float distCovered = (Time.time - startTime) * mortarSpeed;
                float fracJourney = distCovered / journeyLength;
                mortar.transform.position = Vector3.Lerp(initialPosition, targetPosition, fracJourney);
                yield return null;
            }

            if (mortar != null)
            {
                mortar.transform.position = targetPosition;
            }
        }
    }
}
