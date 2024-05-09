using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBuddy : MonoBehaviour
{
    public GameObject buddy;


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            buddy.SetActive(true);
        }
    }
}
