public abstract class PlayerBaseState
{
    public abstract void EnterState(Player_Manager player);
    public abstract void ExitState(Player_Manager player);
    public abstract void UpdateState(Player_Manager player);
}