public abstract class EnemyBaseState
{
    public abstract void EnterState(Enemy enemy);
    public abstract void ExitState(Enemy enemy);
    public abstract void UpdateState(Enemy enemy);
}