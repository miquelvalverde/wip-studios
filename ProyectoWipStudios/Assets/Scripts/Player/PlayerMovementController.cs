using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    //Components
    private CharacterController characterController;
    private Transform cameraTransform;

    //Inputs
    private Vector2 inputDirection;
    private bool inputJump;

    [Header("Turn Movement")]
    [SerializeField] private float turnSmoothTime = .15f;
    private float turnSmoothVelocity;

    [Header("Player Movement")]
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float speedSmoothTime = .1f;
    private float speedSmoothVelocity;
    private float currentSpeed;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpHeight = 1;
    [SerializeField] private float gravity = Physics.gravity.y;
    [SerializeField] private float maxVerticalSpeed = 10;
    private float _maxVerticalSpeed;
    private float verticalSpeed = 0;

    [Header("Grounded Checker")]
    [SerializeField] private float checkerRadius = .4f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector3 checkerOffset = Vector3.up * .2f;
    [SerializeField] private Vector3 chekcerDimensions;

    //Control variables
    private bool doNormalMovement;
    private bool isGrounded;

    public void Initialize(InputSystem controls)
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        controls.Player.Move.performed += ctx => inputDirection = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => inputDirection = Vector2.zero;

        controls.Player.Jump.performed += _ => inputJump = true;
        controls.Player.Jump.canceled += _ => inputJump = false;

        this.ResetMaxVerticalSpeed();
    }

    private void Update()
    {        
        UpdateMovement(true);
        this.Move();
    }

    public void UpdateMovement(bool doNormalMovement)
    {
        this.doNormalMovement = doNormalMovement;

        this.CalculateLookDirection();

        this.CheckIfIsGrounded();

        if (this.doNormalMovement)
            this.CalculateNormalMovement();

        this.CalculateGravity();

        this.CalculateJump();
    }

    private void CalculateLookDirection()
    {
        if(inputDirection != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }
    }

    private void CalculateNormalMovement()
    {
        float targetSpeed = walkSpeed * inputDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
    }

    private void Move()
    {
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * verticalSpeed;
        characterController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

        if (characterController.isGrounded)
            verticalSpeed = 0;
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
        _maxVerticalSpeed = maxVerticalSpeed;
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

        float jumpSpeed = Mathf.Sqrt(-2 * gravity * jumpHeight);
        verticalSpeed = jumpSpeed;
    }
    #endregion

    private void CheckIfIsGrounded()
    {
        isGrounded = Physics.CheckBox(transform.position + checkerOffset, chekcerDimensions, transform.rotation, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(checkerOffset, chekcerDimensions);
    }

    /*public CharacterController characterController { get; private set; }

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float moveTime = .2f;

    [SerializeField] private float maxVerticalSpeed = 10;
    [SerializeField] private float gravity = Physics.gravity.y;

    [Header("Ground detection")]
    [SerializeField] private float groundCheckerRadius = .5f;
    [SerializeField] private LayerMask whatIsGround;

    private float initialMaxVerticalSpeed;

    private Transform camTransform;
    private Vector2 moveInput = Vector2.zero;

    private Vector3 lastForward = Vector3.zero;
    private CollisionFlags collisionFlags;

    [HideInInspector] public float verticalSpeed;
    private Vector3 movement;
    private bool jumpInput;

    private bool hasMovement;

    public struct State
    {
        public Vector3 velocity;
        public bool onGrounded;
        public bool onJump;
    }

    private State currentState;

    public void Initializate(InputSystem controls)
    {
        characterController = this.GetComponent<CharacterController>();

        camTransform = Camera.main.transform;

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;

        controls.Player.Jump.performed += _ => Jump();

        initialMaxVerticalSpeed = maxVerticalSpeed;
    }

    public CollisionFlags MoveWithVelocity(Vector3 velocity)
    {
        return characterController.Move(velocity);
    }

    public bool UpdateMovement()
    {
        currentState.onJump = false;

        collisionFlags = MoveWithVelocity(movement);

        bool onGrounded = CheckGround();

        if ((collisionFlags & CollisionFlags.Below) != 0)
            verticalSpeed = 0;

        if ((collisionFlags & CollisionFlags.Above) != 0 && verticalSpeed > 0f)
            verticalSpeed = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((hasMovement) ? new Vector3(movement.x, 0, movement.z) : lastForward), moveTime * Time.deltaTime);
        lastForward = transform.forward;

        if (onGrounded)
            ResetMaxVerticalSpeed();

        currentState.velocity = characterController.velocity;
        currentState.onGrounded = onGrounded;

        return onGrounded;
    }

    public void UpdateNormalMovement()
    {
        movement = GetMovementVector();
    }

    public void UpdateGravity()
    {
        verticalSpeed += gravity * Time.deltaTime;
        verticalSpeed = Mathf.Clamp(verticalSpeed, -maxVerticalSpeed, Mathf.Infinity);

        MoveWithVelocity(Vector3.up * verticalSpeed * Time.deltaTime);

    }

    private void Jump()
    {
        if (!PlayerController.instance.onGrounded)
            return;

        currentState.onJump = true;
        verticalSpeed = jumpForce;
    }

    public Vector3 GetMovementVector()
    {
        Vector3 movement = Vector3.zero;
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        forward.y = 0;
        forward.Normalize();
        right.y = 0;
        right.Normalize();

        if (moveInput.y > 0)
            movement = forward;
        else if (moveInput.y < 0)
            movement = -forward;

        if (moveInput.x > 0)
            movement += right;
        else if (moveInput.x < 0)
            movement -= right;

        hasMovement = movement != Vector3.zero;
        movement.Normalize();

        movement *= speed * Time.deltaTime;

        return movement;
    }

    private bool CheckGround()
    {
        return Physics.CheckSphere(transform.position + (Vector3.up * .2f), groundCheckerRadius, whatIsGround);
    }

    public State GetState()
    {
        return currentState;
    }

    public void SetLastForward(Vector3 forward)
    {
        lastForward = forward;
    }

    public void SetMaxVerticalSpeed(float maxVerticalSpeed)
    {
        this.maxVerticalSpeed = maxVerticalSpeed;
    }

    public void ResetMaxVerticalSpeed()
    {
        SetMaxVerticalSpeed(initialMaxVerticalSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position + (Vector3.up * .2f), groundCheckerRadius);
    }*/
}
