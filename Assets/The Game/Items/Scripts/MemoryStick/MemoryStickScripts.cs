using UnityEngine;

public class MemoryStickScripts : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            QuestEvents.MemoryStickPickUp();
            Destroy(gameObject);
        }
    }
}
