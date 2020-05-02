using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChameleonController : PlayerSpecificController
{
    [SerializeField] private Tongue tongue = null;
    [SerializeField] private float dropForce;
    [SerializeField] ChameleonHUDController chameleonHUD = null;
    [SerializeField] private Material camouflageMaterial;
    private ChameleonHUDController hudInstance;
    private bool isCamouflaging = false;

    public bool IsCamouflaged { get; private set; }

    public override void Initializate()
    {
        this.controls.Player.Tongue.performed += _ => TonguePick();
        this.controls.Player.Camouflage.performed += _ => Camouflage();
        hudInstance = Instantiate(chameleonHUD);
        camouflageMaterial.SetFloat("Camouflage", 1);
    }

    private void TonguePick()
    {
        tongue.DoTongue(dropForce);
        hudInstance.ShowPickable(tongue.CurrentPickable);
    }

    private void Camouflage()
    {
        if (!isCamouflaging)
            StartCoroutine(CamouflageRoutine(2.0F, IsCamouflaged));
    }

    IEnumerator CamouflageRoutine(float transitionTime, bool reverse)
    {
        isCamouflaging = true;
        float timer = 0.0f;
        while (timer <= transitionTime)
        {
            if (reverse) camouflageMaterial.SetFloat("Camouflage", timer / transitionTime);
            else camouflageMaterial.SetFloat("Camouflage", 1 - timer / transitionTime);
            timer += Time.deltaTime;
            yield return null;
        }
        IsCamouflaged = !IsCamouflaged;
        isCamouflaging = false;

        if (IsCamouflaged)
        {
            this.playerController.useMovementInputs = false;
            this.playerController.lockRotation = true;
            this.playerController.stats.isRunning = true;
            this.playerController.ChangeSpeed(0);
        }
        else
        {
            this.playerController.useMovementInputs = true;
            this.playerController.lockRotation = false;
            this.playerController.stats.isRunning = false;
            this.playerController.ResetSpeed();
        }
    }

    public override void UpdateSpecificAction()
    {
    }

    public override string ToString()
    {
        return "Chameleon";
    }

    private void OnDestroy()
    {
        tongue?.Drop(0);
        if(hudInstance)
            Destroy(hudInstance.gameObject);
    }
}
