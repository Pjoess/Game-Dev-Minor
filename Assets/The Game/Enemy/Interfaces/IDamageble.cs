using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    public int MaxHealthPoints { get; }
    public int HealthPoints { get; set; }
    abstract void Hit(int damage);
    virtual void ApplyKnockback() { }
}
