using UnityEngine;

public class Enemy : EnemyCube
{
    // Override or extend behavior as needed
    protected override void Start()
    {
        base.Start(); // Call base class Start method if needed
    }

    protected override bool IsWeaponCollisionValid(Collider other)
    {
        return base.IsWeaponCollisionValid(other); // Call base class method if needed
    }
}