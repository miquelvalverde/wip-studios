using UnityEngine;

public class PSpecificController : AMonoBehaivourWithInputs
{
    [SerializeField] protected float speed;

    public void DoUpdate()
    {

    }

    public void DoFixedUpdate()
    {

    }

    public float GetSpeed()
    {
        return speed;
    }

}
