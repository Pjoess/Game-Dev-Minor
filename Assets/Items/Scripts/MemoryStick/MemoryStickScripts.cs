using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryStickScripts : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //code for picking up
            Destroy(gameObject);
        }
    }
}
