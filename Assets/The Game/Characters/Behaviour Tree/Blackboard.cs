using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Blackboard : MonoBehaviour, IDamageble
{
    public static Blackboard instance;
    private Player_Manager player;

    private int healthPoints;
    private readonly int maxHealthPoints;
    public int MaxHealthPoints { get { return maxHealthPoints; } }
    public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }

    private int mortarCount = 0; // Counter for mortar cooldown
    private const int maxMortarCount = 3; // Maximum count for mortar cooldown
    private float nextMortarTime = 0f; // Stores the cooldown end time for shooting mortar
    private float mortarCooldownTime = 3f; // Mortar cooldown time in seconds

    public TMP_Text MortarCooldownText; // Text to display the mortar cooldown

    private void Awake()
    {
        player = FindObjectOfType<Player_Manager>();
    }

    private void Start()
    {
        instance = this;
        StartCoroutine(UpdateMortarCooldownText());
    }

    private IEnumerator UpdateMortarCooldownText()
    {
        while (true)
        {
            if (IsMortarOnCooldown())
            {
                float remainingTime = GetMortarCooldownRemaining();
                MortarCooldownText.text = "Cooldown: " + Mathf.CeilToInt(remainingTime) + "s";
            }
            else
            {
                MortarCooldownText.text = "Mortar Ready! - (Press F)";
            }

            yield return new WaitForSeconds(1f); // Wait for 1 second before updating again
        }
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

    // Method to check if shooting mortar is on cooldown
    public bool IsMortarOnCooldown()
    {
        return Time.time < nextMortarTime;
    }

    // Method to update the cooldown end time when shooting mortar
    public void StartMortarCooldown()
    {
        nextMortarTime = Time.time + mortarCooldownTime;
        mortarCount++; // Increment mortar count
        if (mortarCount >= maxMortarCount) // If count reaches maximum, reset count and cooldown time
        {
            mortarCount = 0;
            mortarCooldownTime = 3f; // Reset cooldown time
        }
    }

    // Method to get the remaining time until the mortar cooldown ends
    public float GetMortarCooldownRemaining()
    {
        return nextMortarTime - Time.time;
    }
}
