using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveScript : MonoBehaviour
{

    private Player_Manager player;

    private void Awake()
    {
        player = FindObjectOfType<Player_Manager>();
    }

    private void OnParticleTrigger()
    {
        player.Hit(20);
    }
}
