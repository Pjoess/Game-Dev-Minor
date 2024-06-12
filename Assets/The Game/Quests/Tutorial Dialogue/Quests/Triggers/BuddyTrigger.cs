using UnityEngine;

public class BuddyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TutorialEvents.EnteredBuddy();
        }
    }
}
