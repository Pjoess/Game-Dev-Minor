using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneRoomTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            QuestEvents.EnterThroneRoom();
        }
    }

}
