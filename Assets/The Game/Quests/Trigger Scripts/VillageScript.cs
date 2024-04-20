using UnityEngine;

public class VillageScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            QuestEvents.EnteredVillage();
        }
    }
}
