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
}
