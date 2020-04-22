using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{

    private CharacterController characterController;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float moveTime = .2f;

    private Transform camTransform;
    private Vector2 moveInput = Vector2.zero;

    private Vector3 lastForward = Vector3.zero;
    private CollisionFlags collisionFlags;

    private float verticalSpeed;
    private bool jumpInput;

    public void Initializate(InputSystem controls)
    {
        characterController = this.GetComponent<CharacterController>();

        camTransform = Camera.main.transform;

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;

        controls.Player.Jump.performed += _ => jumpInput = true;
        controls.Player.Jump.canceled += _ => jumpInput = false;

    }

    public bool UpdateMovement()
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

        bool hasMovement = movement != Vector3.zero;
        movement.Normalize();

        movement *= speed * Time.deltaTime;

        verticalSpeed += gravity * Time.deltaTime;
        movement.y = verticalSpeed * Time.deltaTime;

        collisionFlags = characterController.Move(movement);

        bool isGrounded = false;

        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            isGrounded = true;
            verticalSpeed = 0;
        }

        if ((collisionFlags & CollisionFlags.Above) != 0 && verticalSpeed > 0f)
            verticalSpeed = 0;

        if(isGrounded && jumpInput)
        {
            jumpInput = false;

            verticalSpeed = jumpForce;
        }

        transform.forward = (hasMovement) ? Vector3.Lerp(new Vector3(movement.x, 0, movement.z), transform.forward, moveTime) : lastForward;
        lastForward = transform.forward;

        return isGrounded;
    }

}
