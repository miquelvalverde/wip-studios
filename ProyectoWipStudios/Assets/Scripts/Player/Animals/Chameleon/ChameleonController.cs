using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChameleonController : PlayerSpecificController
{
    [SerializeField] private Tongue tongue;

    public override void Initializate(InputSystem controls)
    {
        controls.Player.Tongue.performed += _ => tongue.DoTongue();
    }

    public override void UpdateSpecificAction()
    {
    }
}
