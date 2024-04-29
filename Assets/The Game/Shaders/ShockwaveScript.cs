using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveScript : MonoBehaviour
{

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnParticleTrigger()
    {
        player.Hit(20);
    }
}
