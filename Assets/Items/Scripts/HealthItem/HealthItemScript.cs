using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItemScript : MonoBehaviour
{

    public HealthItem healthItem;

    void Start()
    {
        GetComponent<MeshRenderer>().material.color = healthItem.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().HealPlayer(healthItem.healthRestored);
            Destroy(gameObject);
        }
    }
}
