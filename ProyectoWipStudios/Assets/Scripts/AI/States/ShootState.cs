
public class ShootState : AState
{

    public override void StartState(Enemy self)
    {
        base.StartState(self);
        this.self.agent.isStopped = true;
        player.Dead();
    }

    public override AState ChangeState()
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
