using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerAnimatorController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    //Controllers
    private PlayerMovementController movementController;
    private PlayerAnimatorController animatorController;
    private PlayerCameraController cameraController;
    private PlayerSpecificController _specificController;
    [SerializeField] private RadialMenuController radialMenuController;
    private PlayerSpecificController specificController
    {
        get
        {
            return _specificController;
        }

        set
        {
            _specificController = value;
            animatorController.SetAnimator(_specificController.GetAnimator());
        }

    }

    public InputSystem controls { get; private set; }

    [HideInInspector] public bool doNormalMovement = true;
    [HideInInspector] public bool lockRotation = false;
    [HideInInspector] public bool doGravity = true;

    [HideInInspector] public Vector3 alternativeMoveDestination;

    public float groundDistance
    {
        get
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                return hit.distance;
            }

            return Mathf.Infinity;
        }

        private set { }
    }

    public struct PlayerStats
    {
        public float speed;
        public Vector3 velocity;
        public bool isGrounded;
        public bool isJumping;
        public bool isGliding;
        public bool isClimbing;

        public override string ToString()
        {
            return
                  "Velocity: " + velocity
                + "\nSpeed: " + Mathf.RoundToInt(speed)
                + "\nisGrounded: " + isGrounded
                + "\nisGliding: " + isGliding
                + "\nisClimbing: " + isClimbing;
        }
    }

    [HideInInspector] public PlayerStats stats = new PlayerStats();


    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);

        instance = this;

        Cursor.lockState = CursorLockMode.Locked;

        controls = new InputSystem();
        controls.Enable();

        movementController = this.GetComponent<PlayerMovementController>();
        animatorController = this.GetComponent<PlayerAnimatorController>();
        cameraController = Camera.main.GetComponent<PlayerCameraController>();

        cameraController.Initializate(controls);
        movementController.Initialize(controls);
        radialMenuController.Initializate(controls);
    }

    private void Update()
    {
        radialMenuController.UpdateRadialMenu();

        cameraController.UpdateCamera();

        movementController.UpdateMovement();

        if (specificController)
            specificController.UpdateSpecificAction();

        movementController.Move();

        animatorController.UpdateAnimation();
    }

    /** GETTERS AND SETTERS **/
    public void SetSpecificController(PlayerSpecificController specificController)
    {
        this.specificController = specificController;
    }

    public void ChangeMaxVerticalSpeed(float maxVerticalSpeed)
    {
        this.movementController.SetMaxVerticalSpeed(maxVerticalSpeed);
    }

    public void ResetMaxVerticalSpeed()
    {
        this.movementController.ResetMaxVerticalSpeed();
    }

}