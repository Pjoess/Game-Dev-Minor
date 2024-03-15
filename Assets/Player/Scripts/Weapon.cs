using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private CapsuleCollider swordCollider;
    public AudioSource swordSlashSound;
    public bool colliderSwitchOn_Off = false;
    private List<GameObject> enemies = new List<GameObject>();
    private bool strongAttack = false;

    public bool StrongAttack { get => strongAttack; set => strongAttack = value; }

    void Start()
    {
        swordCollider = GetComponent<CapsuleCollider>();
        swordSlashSound = GetComponent<AudioSource>();
        SwordToDefault();
    }

    // Reset to Default Color + Disable the Collider
    public void SwordToDefault(){
        GetComponent<MeshRenderer>().material.color = new Color32(255, 240, 0, 200); // Yellow Default
        swordCollider.enabled = false; // Disable Collider
        enemies.Clear();
    }

    public void DisableSwordCollider() => swordCollider.enabled = false;
    
    public void DoSwordAttackEnableCollision() 
    {
        swordCollider.enabled = true; // Enable Collision
        swordSlashSound.Play();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (swordCollider.enabled && other.gameObject.CompareTag("Enemy") && !enemies.Any(x => x == other.gameObject))
        {
            other.gameObject.GetComponent<IDamageble>().Hit();
            if(strongAttack)
            {
                other.gameObject.GetComponent<IDamageble>().ApplyKnockback();
            }
            enemies.Add(other.gameObject);
        }
    }
}
