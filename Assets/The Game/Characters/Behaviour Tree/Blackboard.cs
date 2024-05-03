using System;
using UnityEngine;

public class Blackboard : MonoBehaviour, IDamageble
{
    public static Blackboard instance;
    private Player_Manager player;

    private int healthPoints;
    private readonly int maxHealthPoints;
    public int MaxHealthPoints { get { return maxHealthPoints; } }
    public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }
    
    private void Awake()
    {
        player = FindObjectOfType<Player_Manager>();
    }

    private void Start()
    {
        instance = this;
    }

    public Vector3 GetPlayerPosition()
    {
        if (player != null)
        {
            return player.transform.position;
        }
        else
        {
            Debug.LogError("Player_Manager in Blackboard not found!");
            return Vector3.zero;
        }
    }

    public void Hit(int damage)
    {
        player.Hit(damage);
    }
}
