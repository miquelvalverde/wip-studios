using UnityEngine;

public class IdleState : State
{

    private float timer = 0;

    public override State ChangeState()
    {

        if (self.stats.isSeeingPlayer)
            return new ChaseState();

        if (timer < 0)
            return new PatrolState();

        return null;
    }

    public override void ExitState()
    {
        
    }

    public override void StartState(Enemy self)
    {
        base.StartState(self);
        timer = Random.Range(2f, 4f);

        this.self.agent.isStopped = true;

    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
    }
}
