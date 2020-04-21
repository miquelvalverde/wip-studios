using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovementController movementController;
    private PlayerCameraController cameraController = null;

    public InputSystem controls { get; private set; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controls = new InputSystem();
        controls.Enable();

        movementController = this.GetComponent<PlayerMovementController>();
        cameraController = Camera.main.GetComponent<PlayerCameraController>();

        cameraController.Initializate(controls);
    }

    private void Update()
    {
        cameraController.UpdateCamera();
    }


}
