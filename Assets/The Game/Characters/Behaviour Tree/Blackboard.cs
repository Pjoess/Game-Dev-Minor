using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour, IDamageble
{
    public static Blackboard instance;
    private Player_Manager player;

    private int healthPoints;
    private readonly int maxHealthPoints;
    public int MaxHealthPoints { get { return maxHealthPoints; } }
    public int HealthPoints { get { return healthPoints; } set { healthPoints = value; } }

    //private int mortarCount = 0; // Counter for mortar cooldown
    //private readonly int maxMortarCount = 10; // Maximum count for mortar cooldown
    //private float nextMortarTime = 0f; // Stores the cooldown end time for shooting mortar
    //private float mortarCooldownTime = 10; // Mortar cooldown time in seconds

    //public TMP_Text MortarCooldownText; // Text to display the mortar cooldown
    public Slider mortarBar;

    private void Awake()
    {
        player = FindObjectOfType<Player_Manager>();
    }

    private void Start()
    {
        instance = this;
        //StartCoroutine(UpdateMortarCooldownText());
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

    public void Hit(int damage)
    {
        player.Hit(damage);
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

    //OldCode
    //private IEnumerator UpdateMortarCooldownText()
    //{
    //    while (true)
    //    {
    //        if (IsMortarOnCooldown())
    //        {
    //            float remainingTime = GetMortarCooldownRemaining();
    //            MortarCooldownText.text = "Cooldown: " + Mathf.CeilToInt(remainingTime) + "s";
    //        }
    //        else
    //        {
    //            MortarCooldownText.text = "Mortar Ready! - (Press F)";
    //        }

    //        yield return new WaitForSeconds(1f); // Wait for 1 second before updating again
    //    }
    //}

    // Method to check if shooting mortar is on cooldown
    //public bool IsMortarOnCooldown()
    //{
    //    return Time.time < nextMortarTime;
    //}

    //// Method to update the cooldown end time when shooting mortar
    //public void StartMortarCooldown()
    //{
    //    nextMortarTime = Time.time + mortarCooldownTime;
    //    mortarCount++; // Increment mortar count
    //    if (mortarCount >= maxMortarCount) // If count reaches maximum, reset count and cooldown time
    //    {
    //        mortarCount = 0;
    //        mortarCooldownTime = 10f; // Reset cooldown time
    //    }
    //}

    //// Method to get the remaining time until the mortar cooldown ends
    //public float GetMortarCooldownRemaining()
    //{
    //    return nextMortarTime - Time.time;
    //}
    #endregion
}
