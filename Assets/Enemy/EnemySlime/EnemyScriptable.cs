using UnityEngine;


[CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Stats", order = 0)]
public class EnemyScriptable : ScriptableObject 
{
    public float HealthPoints { get; set; }
    public float MaxHealth { get; set; }
    public float MovementSpeed { get; set; }
}