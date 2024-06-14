using buddy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetingNode : IBaseNode
{

    Buddy_Agent buddy;

    public EnemyTargetingNode(Buddy_Agent buddy)
    {
        this.buddy = buddy;
    }

    bool IBaseNode.Update()
    {
        if (buddy.targetedEnemy == null)
        {
            GameObject closestEnemy = GetClosestEnemy();

            if (closestEnemy == null)
            {
                return false;
            }
            else
            {
                closestEnemy.GetComponentInParent<IEnemyMaterialChanger>()?.TargetSlime();
                buddy.targetedEnemy = closestEnemy;
                return true;
            }
        }
        else if (!IsEnemyStillInRange())
        {
            buddy.targetedEnemy.GetComponentInParent<IEnemyMaterialChanger>()?.UnTargetSlime();
            buddy.targetedEnemy = null;
            return false;
        }
        else return true;
    }


    private bool IsEnemyStillInRange()
    {
        float distToEnemy = Vector3.Distance(buddy.transform.position, buddy.targetedEnemy.transform.position);
        if (distToEnemy > buddy.shootingRange) return false;
        else return true;
    }

    private GameObject GetClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestEnemyDistance = Mathf.Infinity;

        Collider[] enemiesInRange = Physics.OverlapSphere(buddy.agent.transform.position, buddy.shootingRange, buddy.attackLayer);

        foreach (Collider enemyCollider in enemiesInRange)
        {
            // Check if the collider belongs to an enemy
            if (enemyCollider.CompareTag("Enemy"))
            {
                GameObject enemyTransform = enemyCollider.gameObject;

                // Check if the enemy is within shooting range
                float distanceToEnemy = Vector3.Distance(buddy.agent.transform.position, enemyTransform.transform.position);
                if (distanceToEnemy <= buddy.shootingRange)
                {
                    if (Physics.Raycast(buddy.agent.transform.position, (enemyTransform.transform.position - buddy.agent.transform.position).normalized, out RaycastHit hit, distanceToEnemy, buddy.attackLayer))
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

        if(closestEnemy != null) return closestEnemy;
        else return null;
    }
}
