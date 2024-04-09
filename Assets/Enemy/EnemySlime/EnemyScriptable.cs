using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Stats", order = 0)]
public class EnemyScriptable : ScriptableObject 
{
    public float MaxHealth;
    public float MovementSpeed;
}