using UnityEngine;

public class PlayerStrikeState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.animator.SetBool(player.animIDStrike, true);
    }

    public override void ExitState(Player player)
    {
        player.animator.SetBool(player.animIDStrike, false);
        player.sword.SwordToDefault();
        player.isStriking = false;
        player.canAttack = false;
    }

    public override void UpdateState(Player player)
    {
        player.Dash();
    }
}

//OldStrikeCode
//public override void EnterState(Player player)
//{
//    Debug.Log("Anim1");
//    player.MoveForwardOnAttack();
//    player.HasAttacked += player.OnAttackStruck;
//    player.animator.SetBool(player.animIDStrike1, true);
//    player.struckAgain = false;
//    player.sword.GetComponent<MeshRenderer>().material.color = new Color32(0, 128, 255, 200); // Blue
//}

//public override void ExitState(Player player)
//{
//    player.HasAttacked -= player.OnAttackStruck;
//    player.sword.SwordToDefault();
//}

//public override void UpdateState(Player player)
//{
//    player.AttackRotation();
//    if (player.IsAnimFinished("Strike1"))
//    {
//        if (player.struckAgain)
//        {
//            player.animator.SetBool(player.animIDStrike2, true);
//            player.ChangeState(player.strike2State);
//        }
//        else
//        {
//            player.animator.SetBool(player.animIDStrike1, false);
//            player.isStriking = false;
//            player.ChangeState(player.idleState);
//        }
//    }
//}
