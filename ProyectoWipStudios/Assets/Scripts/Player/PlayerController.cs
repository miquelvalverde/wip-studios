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
    [SerializeField] private Transform _defaultCameraPoint = null;
    [HideInInspector] public Transform cameraPoint
    {
        get
        {
            return (!specificController) ? _defaultCameraPoint : specificController.cameraPoint;
        }

        private set { }
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

        public override string ToString()
        {
            return
                  "Velocity: " + velocity
                + "\nSpeed: " + Mathf.RoundToInt(speed)
                + "\nisGrounded: " + isGrounded
                + "\nisGliding: " + isGliding
                + "\nisClimbing: " + isClimbing
                + "\nisRunning: " + isRunning
                + "\nisTongue: " + isTongue
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

        cameraController.Initializate();
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

        movementController.Move();

        animatorController.UpdateAnimation();
    }

    /** GETTERS AND SETTERS **/
    public bool IsDoingSomething()
    {
        return this.stats.isClimbing ||
            this.stats.isGliding ||
            this.stats.isRunning;
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


    private void ChangeToAnimal(PlayerSpecificController animalRef)
    {
        if (specificController)
            Destroy(specificController.gameObject);

        specificController = Instantiate(animalRef, transform);
    }

    public void ChangeToSquirrel()
    {
        ChangeToAnimal(squirrelRef);
    }

    public void ChangeToChameleon()
    {
        ChangeToAnimal(chameleonRef);
    }

    public void ChangeToBoar()
    {
        ChangeToAnimal(boarRef);
    }

    public void EnableInputs()
    {
        specificController.EnableControls();
        movementController.EnableControls();
    }

    public void DisableInputs()
    {
        specificController.DisableControls();
        movementController.DisableControls();
    }

}