using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class PlayerSpecificController : MonoBehaviour
{
    protected PlayerController playerController;

    public virtual void Start()
    {
        this.playerController = PlayerController.instance;
        this.playerController.SetSpecificController(this);
        Initializate(this.playerController.controls);
    }

    public abstract void Initializate(InputSystem controls);

    public abstract void UpdateSpecificAction();

    public Animator GetAnimator()
    {
        return this.GetComponent<Animator>();
    }

}
