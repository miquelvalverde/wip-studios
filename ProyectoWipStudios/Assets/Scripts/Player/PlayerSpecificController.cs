using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class PlayerSpecificController : MonoBehaviour
{
    protected PlayerController playerController;
    protected InputSystem controls;


    public float walkSpeed;
    public float jumpHeight;

    public virtual void Awake()
    {
        
        this.controls = new InputSystem();
        Initializate();
    }

    public virtual void Start()
    {
        this.playerController = PlayerController.instance;
        this.playerController.SetSpecificController(this);
    }

    private void OnEnable()
    {
        this.controls.Enable();
    }

    private void OnDisable()
    {
        this.controls.Disable();
    }

    public abstract void Initializate();

    public abstract void UpdateSpecificAction();

    public Animator GetAnimator()
    {
        return this.GetComponent<Animator>();
    }

    public abstract override string ToString();
}
