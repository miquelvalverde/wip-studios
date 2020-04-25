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
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector3 checkerOffset = Vector3.up * .2f;
    [SerializeField] private Vector3 chekcerDimensions;

    //Control variables
    private bool doNormalMovement;
    private bool lockRotation;
    private bool doGravity;
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

    public void UpdateMovement()
    {
        this.UpdateMovement(true, false, true);
    }

    public void UpdateMovement(bool doNormalMovement, bool lockRotation, bool doGravity)
    {
        this.doNormalMovement = doNormalMovement;
        this.lockRotation = lockRotation;
        this.doGravity = doGravity;

        this.CheckIfIsGrounded();

        if(!this.lockRotation)
            this.CalculateLookDirection();

        if (this.doNormalMovement)
            this.CalculateNormalMovement();

        if (this.doGravity)
            this.CalculateGravity();
        else
            verticalSpeed = 0;

        if(this.doNormalMovement)
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

    public void Move()
    {
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * verticalSpeed;
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
