﻿using UnityEngine;

public class PatrolState : State
{
    public override void StartState(Enemy self)
    {
        base.StartState(self);

        this.self.agent.isStopped = false;
        this.self.agent.SetDestination(this.self.GetNextPosition());
        this.self.SetAnim("Walk");
    }

    public override State ChangeState()
    {
        if (self.stats.isSeeingPlayer)
            return new ChaseState();

        if (self.agent.remainingDistance < .5f)
            return new IdleState();

        return null;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
