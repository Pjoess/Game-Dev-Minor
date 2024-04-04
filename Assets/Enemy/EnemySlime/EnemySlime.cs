using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : EnemyBase
{

    public bool isChasingPlayer = false;
    public float chaseRange = 10f;
    [SerializeField]
    public LayerMask playerLayer;
    public Transform centerPoint;


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
        // yield return FacePlayer();

        Vector3 directionToPlayer = Target.position - transform.position;
        // Agent.acceleration = 5;
        Agent.Move(directionToPlayer);
        // Agent.acceleration = 5;

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


    private void CheckChaseRange()
    {
        // Check if player is within chase range
        Collider[] colliders = Physics.OverlapSphere(transform.position, chaseRange, playerLayer);


        // if (colliders.Length > 0)
        //     {
        //         if (colliders[0].gameObject.tag == "Player")
        //         {
        //             print("COLLIDED WITH ENEMY");
        //         }
        //     }
        if (colliders.Length > 0)
        {
            Debug.Log("THERE IS");

            foreach (var collider in colliders){
                // Player detected, chase and attack
                if(collider.gameObject.tag == "Player"){
                    Transform player = colliders[0].transform;
                    Agent.SetDestination(player.position);
                }
                if (!isChasingPlayer) // If not already chasing
                {
                    isChasingPlayer = true;
                }

                // Check if AI is close enough to attack
                if (Vector3.Distance(transform.position, Target.position) <= Agent.stoppingDistance)
                {
                    // Implement attack logic here
                    Attack();
                }
            }

        }
        else
        {
            if (isChasingPlayer) // If stopped chasing
            {
                isChasingPlayer = false;
            }
        }
    }


}
