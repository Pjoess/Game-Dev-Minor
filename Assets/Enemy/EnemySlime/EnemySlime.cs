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

    public override void Hit()
    {
        throw new System.NotImplementedException();
    }

    public override void Idle()
    {
        throw new System.NotImplementedException();
    }


    public IEnumerator AttackCoroutine()
    {
        // FacePlayer();
        yield return new WaitForSeconds(2);

        yield return new WaitForSeconds(3);
        IsAttacking = false;
    }

    // public void FacePlayer()
    // {
    //     Vector3 directionToPlayer = (Target.position - transform.position).normalized;
    //     Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
    //     transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 750);
    // }

}
