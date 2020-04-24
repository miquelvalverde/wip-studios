using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChameleonController : PlayerSpecificController
{
    [SerializeField] private Tongue tongue;
    [SerializeField] private float dropForce;
    [SerializeField] ChameleonHUDController chameleonHUD;
    private ChameleonHUDController hudInstance;

    public override void Initializate(InputSystem controls)
    {
        controls.Player.Tongue.performed += _ => TonguePick();
        hudInstance = Instantiate(chameleonHUD);
    }
        
    private void TonguePick()
    {
        tongue.DoTongue(dropForce);
        hudInstance.ShowPickable(tongue.CurrentPickable);
    }

    public override void UpdateSpecificAction()
    {
    }

    private void OnDestroy()
    {
        tongue?.Drop(0);
        Destroy(hudInstance);
    }
}
