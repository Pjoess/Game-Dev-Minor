using UnityEngine;

public class MemoryStickScripts : MonoBehaviour
{

    private void Start()
    {
        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            QuestEvents.MemoryStickPickUp();
            Destroy(gameObject);
        }
    }
}
