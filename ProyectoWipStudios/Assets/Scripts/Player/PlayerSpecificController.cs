using UnityEngine;

public abstract class PlayerSpecificController : MonoBehaivourWithInputs
{
    protected PlayerController playerController;

    public enum Type { Squirrel, Chameleon, Boar }

    public float walkSpeed;
    public float jumpHeight;
    public float scale = 1;
    public Transform cameraPoint;

    [SerializeField] protected Animator anim = null;

    public virtual void Start()
    {
        this.playerController = PlayerController.instance;
        this.playerController.SetSpecificController(this);
        Initializate();
    }

    public abstract void Initializate();

    public abstract void UpdateSpecificAction();

    public abstract bool CheckIfCanChange(Type to);

    protected bool CheckUp(float distance)
    {
        return !Physics.Raycast(cameraPoint.position, Vector3.up, distance);
    }

    public Animator GetAnimator()
    {
        return anim;
    }

    public abstract override string ToString();
}
