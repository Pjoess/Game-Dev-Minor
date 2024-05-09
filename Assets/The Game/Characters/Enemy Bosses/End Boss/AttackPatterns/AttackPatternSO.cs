using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AttackPattern", menuName = "ScriptableObjects/Attack Pattern")]
public class AttackPatternSO : ScriptableObject
{
    public List<AttackAction> Actions;

    public List<AttackAction> GetActions() { return Actions; }

}
