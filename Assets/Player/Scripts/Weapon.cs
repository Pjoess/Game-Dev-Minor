using UnityEngine;

public class Weapon : MonoBehaviour
{
    private CapsuleCollider swordCollider;
    public bool colliderSwitchOn_Off = false;

    void Start()
    {
        swordCollider = GetComponent<CapsuleCollider>();
        SwordToDefault();
    }

    // Reset to Default Color + Disable the Collider
    public void SwordToDefault(){
        GetComponent<MeshRenderer>().material.color = new Color32(255, 240, 0, 200); // Yellow Default
        swordCollider.enabled = false; // Disable Collider
    }

    public void DisableSwordCollider() => swordCollider.enabled = false;
    public void EnableSwordCollider() => swordCollider.enabled = true;
}