using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{

    public CharacterController characterController { get; private set; }

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

        controls.Player.Jump.performed += _ => jumpInput = true;
        controls.Player.Jump.canceled += _ => jumpInput = false;

        initialMaxVerticalSpeed = maxVerticalSpeed;
    }

    public bool UpdateMovement()
    {

        Vector3 movement = GetMovementVector();
        State nextState = new State();

        collisionFlags = characterController.Move(movement);

        bool onGrounded = CheckGround();

        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            verticalSpeed = 0;
        }

        if ((collisionFlags & CollisionFlags.Above) != 0 && verticalSpeed > 0f)
            verticalSpeed = 0;

        if (jumpInput)
        {
            jumpInput = false;
            nextState.onJump = true;
            if (onGrounded)
                verticalSpeed = jumpForce;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((hasMovement) ? new Vector3(movement.x, 0, movement.z) : lastForward), moveTime * Time.deltaTime);
        lastForward = transform.forward;

        if (onGrounded)
            ResetMaxVerticalSpeed();

        nextState.velocity = characterController.velocity;
        nextState.onGrounded = onGrounded;

        currentState = nextState;

        return onGrounded;
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

        verticalSpeed += gravity * Time.deltaTime;
        verticalSpeed = Mathf.Clamp(verticalSpeed, -maxVerticalSpeed, maxVerticalSpeed);
        movement.y = verticalSpeed * Time.deltaTime;

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
    }

}
