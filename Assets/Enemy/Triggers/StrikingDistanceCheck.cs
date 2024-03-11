using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikingDistanceCheck : MonoBehaviour
{

    public GameObject PlayerTarget { get; set; }
    private Enemy enemy;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");

        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject == PlayerTarget)
        {
            enemy.SetStrikingDistanceBool(true);
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject == PlayerTarget)
        {
            enemy.SetStrikingDistanceBool(false);
        }
    }
}
