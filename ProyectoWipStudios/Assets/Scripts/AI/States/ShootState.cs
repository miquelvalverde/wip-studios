
public class ShootState : State
{

    public override void StartState(Enemy self)
    {
        base.StartState(self);
        this.self.agent.isStopped = true;
        this.self.SetAnim("Shoot");
        player.Dead();
    }

    public override State ChangeState()
    {
        return null;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
