using UnityEngine;

public class PlayerStrike3State : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.MoveForwardOnAttack();
        player.sword.DoSwordAttackEnableCollision();
        Debug.Log("Anim3");
        player.sword.GetComponent<MeshRenderer>().material.color = new Color32(204, 0, 0, 200); // Red
        player.sword.StrongAttack = true;
    }

    public override void ExitState(Player player)
    {
        player.sword.DisableSwordCollider();
        player.animator.SetBool(player.animIDStrike1, false);
        player.animator.SetBool(player.animIDStrike2, false);
        player.animator.SetBool(player.animIDStrike3, false);
        player.sword.SwordToDefault();
        player.sword.StrongAttack = false;
        player.isStriking = false;
    }

    public override void UpdateState(Player player)
    {
        player.AttackRotation();
        if (player.IsAnimFinished("Strike3")) player.ChangeState(player.idleState);
    }
}
