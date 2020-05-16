
public class ChaseState : State
{

    public override void StartState(Enemy self)
    {
        base.StartState(self);

        this.self.agent.isStopped = false;
    }

    public override State ChangeState()
    {
        if (self.stats.isPlayerFar)
            return new IdleState();

        if (self.stats.isPlayerClose)
            return new ShootState();

        return null;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        self.agent.SetDestination(player.transform.position);
    }
}
