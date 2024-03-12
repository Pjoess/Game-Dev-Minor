using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroCheck : MonoBehaviour
{

    public GameObject PlayerTarget { get; set; }
    private NewEnemy enemy;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");

        enemy = GetComponentInParent<NewEnemy>();
    }

    private void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject == PlayerTarget)
        {
            enemy.SetAggroStatus(true);
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject == PlayerTarget)
        {
            enemy.SetAggroStatus(false);
        }
    }
}
