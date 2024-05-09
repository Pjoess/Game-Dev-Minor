using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackAction
{
    [HideInInspector] public enum Action { BULLET, MORTAR, WAIT }

    public Action action;
    public float interval;

    public Action GetAction() { return action; }
    public float GetInterval() { return interval; }
}
