using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveScript : MonoBehaviour
{

    private Player_Manager player;
    private ParticleSystem _particleSystem;

    private bool hasHit = false;

    private void Awake()
    {
        player = FindObjectOfType<Player_Manager>();
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.trigger.AddCollider(player);
    }

    private void OnParticleTrigger()
    {
        if (!hasHit) 
        {
            player.Hit(20);
            hasHit = true;
            Vector3 knockbackPos = transform.position;

            //Set the same y position so player doesn't start flying :)
            knockbackPos.y = player.transform.position.y;

            player.ApplyKnockback(knockbackPos, 500);
        }
    }

    private void Update()
    {
       if ( !_particleSystem.IsAlive() )
        {
            hasHit = false;
        }
    }
}
