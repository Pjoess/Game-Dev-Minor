using UnityEngine;

public class Blackboard : MonoBehaviour
{
    public static Blackboard instance;
    private Player_Manager player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<Player_Manager>();
    }

    public Vector3 GetPlayerPosition()
    {
        if (player != null)
        {
            return player.transform.position;
        }
        else
        {
            Debug.LogError("Player_Manager not found!");
            return Vector3.zero;
        }
    }
}
