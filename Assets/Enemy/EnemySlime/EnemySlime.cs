using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : EnemyBase
{

    public override void InitializeStates()
    {

    }

    public override void Attack()
    {
        Agent.isStopped = true;
        IsAttacking = true;

        StartCoroutine(AttackCoroutine());
    }

    public override void Chase()
    {
        SetAgentDestination();

        //animations

        //effects

    }

    public override void Hit(float damage)
    {
        HealthPoints -= damage;

        enemyHealthBar.UpdateHealthBar(HealthPoints,MaxHealth);

        if(HealthPoints <= 0)
        {
            Destroy(this.gameObject);
        }


    }

    public override void Idle()
    {
        throw new System.NotImplementedException();
    }


    public IEnumerator AttackCoroutine()
    {
        yield return FacePlayer();

        // Agent.acceleration = 100;
        // Agent.Move()

        yield return new WaitForSeconds(3);
        IsAttacking = false;
    }

    public IEnumerator FacePlayer()
    {
        Vector3 directionToPlayer = (Target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
        yield return null;
    }



    public void OnTriggerEnter(Collider other)
    {
        if (IsWeaponCollisionValid(other))
        {
            float damage = other.GetComponentInParent<Player>().attackDamage;

            Hit(damage);
        }
    }

    public bool IsWeaponCollisionValid(Collider other)
    {
        return other.gameObject.CompareTag("Weapon")&& !isCollisionCooldown;
    }

    public void StartCollisionCooldown()
    {
        isCollisionCooldown = true;
        Invoke(nameof(EndCollisionCooldown), collisionCooldown);
    }

    public void EndCollisionCooldown() => isCollisionCooldown = false;
}
