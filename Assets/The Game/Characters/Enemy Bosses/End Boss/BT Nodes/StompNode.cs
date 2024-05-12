using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompNode : IBaseNode
{

    private FinalBoss boss;
    private float cooldown = 0;

    public StompNode( FinalBoss boss )
    {
        this.boss = boss;
    }


    public bool Update()
    {
        if(cooldown > 0) cooldown -= Time.deltaTime;
        else
        {
            float distanceToPlayer = Vector3.Distance(boss.transform.position, Blackboard.instance.GetPlayerPosition());
            if (distanceToPlayer < boss.meleeRange)
            {
                if (boss.IsAnimatorCurrentState("idle"))
                {
                    boss.animator.SetBool(boss.animIDIsStomping, true);
                    cooldown = boss.stompCooldown;
                }
            }
        }

        return true;
    }
}
