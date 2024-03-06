using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        Debug.Log("Player starts swing...");
    }

    public override void ExitState(Player player)
    {
        Debug.Log("Player finish swing...");
    }

    public override void UpdateState(Player player)
    {
        Debug.Log("Player check if hits something...");
    }
}