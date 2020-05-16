public abstract class State
{

    protected PlayerController player
    {
        get { return PlayerController.instance; }
    }

    protected Enemy self;

    public abstract void UpdateState();

    public abstract State ChangeState();

    public virtual void StartState(Enemy self)
    {
        this.self = self;
    }

    public abstract void ExitState();

}
