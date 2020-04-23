using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    private PlayerMovementController movementController;
    private PlayerCameraController cameraController;
    private PlayerSpecificController specificController;
    [SerializeField] private RadialMenuController radialMenuController;

    public InputSystem controls { get; private set; }

    [HideInInspector] public bool canNormalMove;

    public bool onGrounded { get; private set; }

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

    public Vector3 movementVector
    {
        get
        {
            return this.movementController.GetMovementVector();
        }

        private set { }
    }

    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);

        instance = this;

        Cursor.lockState = CursorLockMode.Locked;

        controls = new InputSystem();
        controls.Enable();

        movementController = this.GetComponent<PlayerMovementController>();
        cameraController = Camera.main.GetComponent<PlayerCameraController>();

        cameraController.Initializate(controls);
        movementController.Initializate(controls);
        radialMenuController.Initializate(controls);

        canNormalMove = true;
    }

    private void Update()
    {
        radialMenuController.UpdateRadialMenu();

        if (radialMenuController.IsHoldingToChange)
            return;

        if (canNormalMove)
        {
            movementController.characterController.enabled = true;
            onGrounded = movementController.UpdateMovement();
        }
        else
            movementController.characterController.enabled = false;

        if (specificController)
            specificController.UpdateSpecificAction();

    }

    private void LateUpdate()
    {
        if (radialMenuController.IsHoldingToChange)
            return;

        cameraController.UpdateCamera();
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