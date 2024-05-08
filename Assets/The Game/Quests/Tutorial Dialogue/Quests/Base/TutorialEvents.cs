using System;
using UnityEngine;

public class TutorialEvents : MonoBehaviour
{
    public static event Action OnEnterCamera;
    public static event Action OnEnterAttack;
    public static event Action OnEnterDodge;
    public static event Action OnEnterBuddy;
    public static event Action OnKillEnemies;

    public static void EnteredCamera()
    {
        OnEnterCamera?.Invoke();
    }

    public static void EnteredAttack()
    {
        OnEnterAttack?.Invoke();
    }

    public static void EnteredDodge()
    {
        OnEnterDodge?.Invoke();
    }

    public static void EnteredBuddy()
    {
        OnEnterBuddy?.Invoke();
    }

    public static void KilledEnemies()
    {
        OnKillEnemies?.Invoke();
    }
}
