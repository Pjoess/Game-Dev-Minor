using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private CapsuleCollider swordCollider;
    public AudioSource swordSlashSound;
    public bool colliderSwitchOn_Off = false;
    private readonly List<GameObject> enemiesHit = new();
    private bool strongAttack = false;

    [SerializeField] ParticleSystem hitParticle;

    public bool StrongAttack { get => strongAttack; set => strongAttack = value; }

    public event Action onWeaponHit;

    void Start()
    {
        gameObject.GetComponentInChildren<ParticleSystem>().Stop();
        swordCollider = GetComponent<CapsuleCollider>();
        swordSlashSound = GetComponent<AudioSource>();
        SwordToDefault();
    }

    // Reset to Default Color + Disable the Collider
    public void SwordToDefault(){
        gameObject.GetComponentInChildren<ParticleSystem>().Stop();
        //GetComponent<MeshRenderer>().material.color = new Color32(255, 240, 0, 200); // Yellow Default
        swordCollider.enabled = false; // Disable Collider
        enemiesHit.Clear();
    }

    public void DisableSwordCollider() => swordCollider.enabled = false;
    
    public void DoSwordAttackEnableCollision() 
    {
        gameObject.GetComponentInChildren<ParticleSystem>().Play();
        swordCollider.enabled = true; // Enable Collision
        swordSlashSound.Play();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (swordCollider.enabled && other.gameObject.CompareTag("Enemy") && !enemiesHit.Any(x => x == other.gameObject))
        {
            other.gameObject.GetComponentInParent<IDamageble>().Hit(5);
            Instantiate(hitParticle, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
            onWeaponHit?.Invoke();
            if(strongAttack)
            {
                //other.gameObject.GetComponent<IDamageble>().ApplyKnockback();
            }
            enemiesHit.Add(other.gameObject);
            TimeScript.instance.HitStop();
        }
    }
}
