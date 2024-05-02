using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItemScript : MonoBehaviour
{

    public HealthItem healthItem;

    void Start()
    {
        GetComponent<MeshRenderer>().material.color = healthItem.color;
        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player_Manager>().HealPlayer(healthItem.healthRestored);
            Destroy(transform.parent.gameObject);
        }
    }
}
