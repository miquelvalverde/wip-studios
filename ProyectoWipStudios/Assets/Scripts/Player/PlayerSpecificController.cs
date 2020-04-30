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
    public float scale = 1;
    public Transform cameraPoint;

    public virtual void Awake()
    {
        this.controls = new InputSystem();
    }

    public virtual void Start()
    {
        this.playerController = PlayerController.instance;
        this.playerController.SetSpecificController(this);
        Initializate();
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
