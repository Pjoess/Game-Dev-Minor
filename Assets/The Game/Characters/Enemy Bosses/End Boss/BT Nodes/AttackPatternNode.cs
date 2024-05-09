using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatternNode : IBaseNode
{

    private FinalBoss boss;
    private List<AttackAction> currentPattern;
    private int index = 0;
    private float attackIntervalWaitTime = 0f;
    private float waitAfterPatternFinishTime = 0f;

    public AttackPatternNode(FinalBoss boss)
    {
        this.boss = boss;
    }

    //return values are meant for the stomp attack. The stomp attack should only happen if
    //Boss is not attacking a player is too close.
    public bool Update()
    {
        if(boss.isAttacking)
        {
            if (attackIntervalWaitTime > 0)
            {
                attackIntervalWaitTime -= Time.deltaTime;
                //While it's not doing anything, boss is still in a pattern so return true
                return true;
            }
            else
            {
                Attack();
                //Currently attacking, return true
                return true;
            }
        }
        else 
        {
            if (waitAfterPatternFinishTime > 0)
            {
                waitAfterPatternFinishTime -= Time.deltaTime;
                //Is not currently attacking, return false
                return false;
            }
            else
            {
                LoadPattern();
                //Will start attacking so return true
                return true;
            }
        }
    }


    private void LoadPattern()
    {
        currentPattern = boss.GetRandomPattern();
        index = 0;
        boss.isAttacking = true;
    }

    private void Attack()
    {
        AttackAction.Action action = currentPattern[index].GetAction();

        switch (action)
        {
            case AttackAction.Action.BULLET:
                {
                    DoBulletAttack();
                    break;
                }

            case AttackAction.Action.MORTAR:
                {
                    DoMortarAttack();
                    break;
                }

            case AttackAction.Action.WAIT:
                {
                    //Only Interval so no code needed
                    break;
                }
        }

        if (index == currentPattern.Count - 1)
        {
            waitAfterPatternFinishTime = boss.attackPatternIntervalTime;
            boss.isAttacking = false;
        }
        else
        {
            attackIntervalWaitTime = currentPattern[index].GetInterval();
            index++;
        }
    }

    private void DoBulletAttack()
    {
        Object.Instantiate(boss.bulletPrefab, boss.transform.position + Vector3.up * 3f, Quaternion.identity);
    }

    private void DoMortarAttack()
    {
        Object.Instantiate(boss.mortarPrefab, Blackboard.instance.GetPlayerPosition() + Vector3.up * 8f, Quaternion.identity);
    }
}
