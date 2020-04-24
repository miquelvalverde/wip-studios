using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChameleonController : PlayerSpecificController
{
    [SerializeField] private Tongue tongue;
    [SerializeField] private float dropForce;
        
    public override void Initializate(InputSystem controls)
    {
        controls.Player.Tongue.performed += _ => tongue.DoTongue(dropForce);
    }

    public override void UpdateSpecificAction()
    {
    }

    private void OnDestroy()
    {
        tongue?.Drop(0);
    }
}
