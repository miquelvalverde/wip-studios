using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class PlayerSpecificController : MonoBehaviour
{
    protected PlayerController playerController;

    public float walkSpeed;
    public float jumpHeight;

    public virtual void Start()
    {
        this.playerController = PlayerController.instance;
        this.playerController.SetSpecificController(this);
        Initializate(this.playerController.controls);
        this.playerController.controls.Enable();
    }

    public abstract void Initializate(InputSystem controls);

    public abstract void UpdateSpecificAction();

    public Animator GetAnimator()
    {
        return this.GetComponent<Animator>();
    }
}
