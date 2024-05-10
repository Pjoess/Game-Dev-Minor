using UnityEngine;

public class CastleScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            QuestEvents.ReachedCastle();
        }
    }
}
