using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITriggerCheckable
{
    bool IsAggroed { get; set; }
    bool IsWithinStrikingDistance { get; set; }
    IEnumerator IsAttacking { get; set; }
    void SetAggroStatus(bool isAggroed);
    void SetStrikingDistanceBool(bool isWithinStrikingDistance);
    void SetIsAttackingBool(bool isAttacking);
}