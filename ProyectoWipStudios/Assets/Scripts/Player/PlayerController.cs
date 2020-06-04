using System.Threading;
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
    [SerializeField] private RadialMenuController radialMenuController = null;

    public RadialMenuController GetRadialMenuController { get => radialMenuController; }

    [SerializeField] private Transform _defaultCameraPoint = null;
    [HideInInspector] public Transform cameraPoint
    {
        get
        {
            return (!specificController) ? _defaultCameraPoint : specificController.cameraPoint;
        }
    }

    [HideInInspector]
    public Transform headPoint
    {
        get
        {
            return (!specificController) ? null : specificController.headPoint;
        }
    }

    private float characterDefaultHeight;
    private float characterDefaultYCenter;


    public PlayerSpecificController specificController
    {
        get
        {
            return _specificController;
        }

        private set
        {
            _specificController = value;
            animatorController.SetAnimator(_specificController.GetAnimator());
            movementController.SetSpeedAndJumpHeight(_specificController.walkSpeed, _specificController.jumpHeight);
        }

    }

    [HideInInspector] public bool doNormalMovement = true;
    [HideInInspector] public bool useMovementInputs = true;
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
        public bool isRunning;
        public bool isTongue;
        public bool isDead;
        public bool isCamouflaged;
        
        public override string ToString()
        {
            return
                  "Velocity: " + velocity
                + "\nSpeed: " + Mathf.RoundToInt(speed)
                + "\nisDead: " + isDead
                + "\nisGrounded: " + isGrounded
                + "\nisGliding: " + isGliding
                + "\nisClimbing: " + isClimbing
                + "\nisRunning: " + isRunning
                + "\nisTongue: " + isTongue
                + "\nisCamouflaged: " + isCamouflaged
                + "\nAnimal: " + PlayerController.instance.specificController.ToString();
        }
    }

    [HideInInspector] public PlayerStats stats = new PlayerStats();


    [Header("Animals objects")]
    [SerializeField] private PlayerSpecificController squirrelRef = null;
    [SerializeField] private PlayerSpecificController chameleonRef = null;
    [SerializeField] private PlayerSpecificController boarRef = null;

    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);

        instance = this;

        Cursor.lockState = CursorLockMode.Locked;

        movementController = this.GetComponent<PlayerMovementController>();
        animatorController = this.GetComponent<PlayerAnimatorController>();
        cameraController = Camera.main.GetComponent<PlayerCameraController>();

        movementController.Initialize();
        radialMenuController.Initializate();

        characterDefaultHeight = movementController.characterController.height;
        characterDefaultYCenter = movementController.characterController.center.y;

        ChangeToSquirrel();
    }

    private void Update()
    {
        radialMenuController.UpdateRadialMenu();

        cameraController.UpdateCamera();

        movementController.UpdateMovement();

        if (specificController)
            specificController.UpdateSpecificAction();

        animatorController.UpdateAnimation();
    }

    private void FixedUpdate()
    {
        movementController.Move();
    }

    /** GETTERS AND SETTERS **/
    public bool IsDoingSomething()
    {
        return this.stats.isClimbing ||
            this.stats.isGliding ||
            this.stats.isRunning ||
            this.stats.isTongue ||
            this.stats.isCamouflaged;
    }

    public void SetSpecificController(PlayerSpecificController specificController)
    {
        this.specificController = specificController;
        CharacterController cc = GetComponent<CharacterController>();
        cc.height = characterDefaultHeight * this.specificController.scale;
        cc.center = new Vector3(0, characterDefaultYCenter * this.specificController.scale, 0);
    }

    public void ChangeMaxVerticalSpeed(float maxVerticalSpeed)
    {
        this.movementController.SetMaxVerticalSpeed(maxVerticalSpeed);
    }

    public void ResetMaxVerticalSpeed()
    {
        this.movementController.ResetMaxVerticalSpeed();
    }

    public void ChangeSpeed(float speed)
    {
        movementController.ChangeSpeed(speed);
    }

    public void ResetSpeed()
    {
        movementController.ResetSpeed();
    }


    private void ChangeToAnimal(PlayerSpecificController animalRef, PlayerSpecificController.Type newAnimalType)
    {
        if (specificController)
        {
            if (!specificController.CheckIfCanChange(newAnimalType))
                return;

            Destroy(specificController.gameObject);
        }

        specificController = Instantiate(animalRef, transform);
    }

    public void ChangeToSquirrel()
    {
        ChangeToAnimal(squirrelRef, PlayerSpecificController.Type.Squirrel);
    }

    public void ChangeToChameleon()
    {
        ChangeToAnimal(chameleonRef, PlayerSpecificController.Type.Chameleon);
    }

    public void ChangeToBoar()
    {
        ChangeToAnimal(boarRef, PlayerSpecificController.Type.Boar);
    }

    public void EnableInputs()
    {
        specificController.EnableControls();
        movementController.EnableControls();
        radialMenuController.EnableControls();
        cameraController.EnableControls();
    }

    public void DisableInputs()
    {
        specificController.DisableControls();
        movementController.DisableControls();
        radialMenuController.DisableControls();
    }
        
    public void DisableSpecificController()
    {
        specificController.DisableControls();
    }

    public void DisableCameraControls()
    {
        cameraController.DisableControls();
    }

    public void TeleportTo(Transform point)
    {
        if (!point)
            return;

        movementController.TeleportTo(point.position);
    }

    public void Dead()
    {
        DisableInputs();
        cameraController.DisableControls();
        radialMenuController.DisableControls();
        DeadMenuHUDController.instance.DisplayDeadMenu();
        stats.isDead = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SpeedToZero()
    {
        this.movementController.SpeedToZero();
    }

}