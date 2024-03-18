using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    public int HealthPoints { get; set; }
    void Hit();
    virtual void ApplyKnockback(){}
}
