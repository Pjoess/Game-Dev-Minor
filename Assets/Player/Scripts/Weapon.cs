using UnityEngine;

public class Weapon : MonoBehaviour
{
    private CapsuleCollider swordCollider;
    public bool colliderSwitch = false;

    void Start()
    {
        swordCollider = GetComponent<CapsuleCollider>();
        DisableSwordCollider();
    }

    public void SwitchSwordCollider()
    {
        if (colliderSwitch) EnableSwordCollider();
        else DisableSwordCollider();
    }

    public void DisableSwordCollider() => swordCollider.enabled = false;
    public void EnableSwordCollider() => swordCollider.enabled = true;
}