using System;
using UnityEngine;

public class QuestEvents : MonoBehaviour
{
    public static event Action OnPlayerEnterVillage;
    public static event Action OnPlayerReachCastle;
    public static event Action OnMemoryStickPickUp;

    public static event Action OnBuddyDeath;
    public static event Action OnPlayerEnterCastle;

    public static void EnteredVillage()
    {
        Debug.Log("Entered Village");
        OnPlayerEnterVillage?.Invoke();
    }

    public static void ReachedCastle()
    {
        Debug.Log("Reached Castle");
        OnPlayerReachCastle?.Invoke();
    }

    public static void MemoryStickPickUp()
    {
        Debug.Log("Picked Up Stick");
        OnMemoryStickPickUp?.Invoke();
    }

    public static void BuddyDead()
    {
        OnBuddyDeath?.Invoke();
    }

    public static void EnterCastle()
    {
        OnPlayerEnterCastle?.Invoke();
    }
}
