using UnityEngine;

public class PlayerStrikeState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.sword.DoSwordAttackEnableCollision();
        player.HasAttacked += player.OnAttackStruck;
        player.animator.SetBool(player.animIDStrike1, true);
        player.struckAgain = false;
        Debug.Log("Anim1");
        player.sword.GetComponent<MeshRenderer>().material.color = new Color32(0, 128, 255, 200); // Blue
    }

    public override void ExitState(Player player)
    {
        player.HasAttacked -= player.OnAttackStruck;
        player.sword.SwordToDefault();
    }

    public override void UpdateState(Player player)
    {
        player.AttackRotation();
        if (player.IsAnimFinished("Strike1"))
        {
            if (player.struckAgain)
            {
                player.animator.SetBool(player.animIDStrike2, true);
                player.ChangeState(player.strike2State);
            }
            else
            {
                player.animator.SetBool(player.animIDStrike1, false);
                player.ChangeState(player.idleState);
            }
        }
    }
}
