using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    //Components
    public CharacterController characterController { get; private set; }
    private Transform cameraTransform;

    //Inputs
    private InputSystem controls;
    private Vector2 inputDirection;
    private bool inputJump;

    [Header("Turn Movement")]
    [SerializeField] private float turnSmoothTime = .15f;
    private float turnSmoothVelocity;

    [Header("Player Movement")]
    [SerializeField] private float speedSmoothTime = .1f;
    private float _speed = 5;
    private float speed = 5;
    private float speedSmoothVelocity;
    private float currentSpeed;

    [Header("Vertical Movement")]
    [SerializeField] private float gravity = Physics.gravity.y;
    [SerializeField] private float maxVerticalSpeed = 10;
    private float _jumpHeight = 1;
    private float jumpHeight = 1;
    private float _maxVerticalSpeed;
    private float verticalSpeed = 0;

    [Header("Grounded Checker")]
    [SerializeField] private LayerMask whatIsGround = 0;
    [SerializeField] private Vector3 checkerOffset = Vector3.up * .2f;
    [SerializeField] private Vector3 chekcerDimensions = Vector3.one;

    //Control variables
    private bool doNormalMovement;
    private bool lockRotation;
    private bool doGravity;
    private bool useMovementInputs;

    private bool isGrounded;

    private void Awake()
    {
        controls = new InputSystem();

        controls.Player.Move.performed += ctx => inputDirection = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => inputDirection = Vector2.zero;

        controls.Player.Jump.performed += _ => inputJump = true;
        controls.Player.Jump.canceled += _ => inputJump = false;
    }

    public void Initialize()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        
        this.ResetMaxVerticalSpeed();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void UpdateMovement()
    {
        this.doNormalMovement = PlayerController.instance.doNormalMovement;
        this.lockRotation = PlayerController.instance.lockRotation;
        this.doGravity = PlayerController.instance.doGravity;
        this.useMovementInputs = PlayerController.instance.useMovementInputs;

        this.CheckIfIsGrounded();

        if (!this.lockRotation)
            this.CalculateLookDirection();

        if (this.doNormalMovement)
            this.CalculateNormalMovement();

        if (this.doGravity)
            this.CalculateGravity();
        else
            verticalSpeed = 0;

        if (this.doNormalMovement && this.useMovementInputs)
            this.CalculateJump();

        if (inputJump)
            inputJump = false;
    }

    private void CalculateLookDirection()
    {
        if (inputDirection != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }
    }

    private void CalculateNormalMovement()
    {
        float targetSpeed = speed;
        if (PlayerController.instance.useMovementInputs)
            targetSpeed *= inputDirection.magnitude;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
    }

    public void Move()
    {
        Vector3 velocity;
        velocity = (!PlayerController.instance.stats.isClimbing) ?
            transform.forward * currentSpeed + Vector3.up * verticalSpeed :
            (PlayerController.instance.alternativeMoveDestination - transform.position) * speed;

        characterController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

        if (characterController.isGrounded)
            verticalSpeed = 0;

        PlayerController.instance.stats.isGrounded = isGrounded;
        PlayerController.instance.stats.speed = currentSpeed;
        PlayerController.instance.stats.velocity = velocity;
    }

    #region Gravity
    private void CalculateGravity()
    {
        if (!isGrounded)
        {
            verticalSpeed += -Mathf.Abs(gravity) * Time.deltaTime;
            verticalSpeed = Mathf.Clamp(verticalSpeed, -Mathf.Abs(_maxVerticalSpeed), Mathf.Infinity);
        }
    }
    public void ResetMaxVerticalSpeed()
    {
        SetMaxVerticalSpeed(maxVerticalSpeed);
    }

    public void SetMaxVerticalSpeed(float newSpeed)
    {
        _maxVerticalSpeed = newSpeed;
    }
    #endregion

    #region Jump
    private void CalculateJump()
    {
        if (inputJump)
            Jump();
    }

    private void Jump()
    {
        inputJump = false;

        if (!doNormalMovement || !isGrounded)
            return;

        float jumpSpeed = Mathf.Sqrt(-2 * -Mathf.Abs(gravity) * jumpHeight);
        verticalSpeed = jumpSpeed;

        PlayerController.instance.stats.isJumping = true;
    }
    #endregion

    public void SetSpeedAndJumpHeight(float speed, float jumpHeight)
    {
        this.speed = this._speed = speed;
        this.jumpHeight = _jumpHeight = jumpHeight;
    }

    public void ChangeSpeed(float speed)
    {
        this.speed = speed;
    }

    public void ResetSpeed()
    {
        this.speed = this._speed;
    }

    private void CheckIfIsGrounded()
    {
        isGrounded = Physics.CheckBox(transform.position + checkerOffset, chekcerDimensions/2, transform.rotation, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(checkerOffset, chekcerDimensions);
    }
}
