using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour
{
    public static Blackboard instance;
    private Player_Manager player;

    private int healthPoints;
    private readonly int maxHealthPoints;
    public int MaxHealthPoints { get { return maxHealthPoints; } }
    public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }

    public Slider mortarBar;

    private void Awake()
    {
        player = FindObjectOfType<Player_Manager>();
    }

    private void Start()
    {
        instance = this;
    }

    #region Player
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
    #endregion

    public void HitPlayer(int damage, Vector3 position)
    {
        player.Hit(damage);
        player.ApplyKnockback(position, 150);
    }

    #region Buddy


    public bool IsMortarReady()
    {
        //add the > just to be sure
        return mortarBar.value >= mortarBar.maxValue;
    }

    public void AddToMortarBar(int value)
    {
        if(mortarBar != null)
        {
            mortarBar.value += value;
        }
    }
    public void ResetMortar()
    {
        mortarBar.value = 0f;
    }
    #endregion
}
