using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TutorialEvents.EnteredCamera();
        }
    }
}
