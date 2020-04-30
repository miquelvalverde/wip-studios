using UnityEngine;

public abstract class PlayerSpecificController : AMonoBehaivourWithInputs
{
    protected PlayerController playerController;

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

    public Animator GetAnimator()
    {
        return anim;
    }

    public abstract override string ToString();
}
