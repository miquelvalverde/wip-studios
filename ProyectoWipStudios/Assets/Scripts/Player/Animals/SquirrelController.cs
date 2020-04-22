using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : PlayerSpecificController
{

    private bool glideInput;

    [SerializeField] private float maxVerticalSpeedGlide = 2f;

    public override void Initializate(InputSystem controls)
    {
        controls.Player.Glide.performed += _ => glideInput = true;
        controls.Player.Glide.canceled += _ => glideInput = false;
    }

    public override void UpdateSpecificAction()
    {
        if (glideInput && this.playerController.groundDistance > 1f && this.playerController.movementVector.y < 0)
        {
            this.playerController.ChangeMaxVerticalSpeed(maxVerticalSpeedGlide);
        }
        else if (!glideInput)
            this.playerController.ResetMaxVerticalSpeed();
    }
}
