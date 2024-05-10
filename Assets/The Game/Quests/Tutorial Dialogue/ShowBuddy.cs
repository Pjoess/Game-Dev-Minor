using UnityEngine;
using UnityEngine.AI;

public class ShowBuddy : MonoBehaviour
{
    public GameObject buddy;
    private NavMeshAgent buddyAgent;
    private bool isFrozen = true;

    private void Start()
    {
        // Cache the NavMeshAgent component of the buddy
        buddyAgent = buddy.GetComponent<NavMeshAgent>();
        FreezeBuddy();
    }

    private void Update()
    {
        // If the buddy is not frozen and not currently moving, freeze it
        if (!isFrozen && !buddyAgent.hasPath)
        {
            FreezeBuddy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isFrozen)
            {
                // Unfreeze the buddy
                UnfreezeBuddy();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isFrozen)
            {
                FreezeBuddy();
            }
        }
    }

    private void FreezeBuddy()
    {
        // Stop the buddy's movement
        buddyAgent.isStopped = true;
        isFrozen = true;
    }

    private void UnfreezeBuddy()
    {
        // Allow the buddy to move
        buddyAgent.isStopped = false;
        isFrozen = false;
    }
}
