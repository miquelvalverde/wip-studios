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
    [SerializeField] private float transitionTime = 1.5F;
    private ChameleonHUDController hudInstance;
    private bool isCamouflaging = false;
        
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
            StartCoroutine(CamouflageRoutine(this.playerController.stats.isCamouflaged));
    }

    IEnumerator CamouflageRoutine(bool reverse)
    {
        this.playerController.stats.isCamouflaged = !this.playerController.stats.isCamouflaged;

        isCamouflaging = true;
        float timer = 0.0f;
        while (timer <= transitionTime)
        {
            if (reverse) camouflageMaterial.SetFloat("Camouflage", timer / transitionTime);
            else camouflageMaterial.SetFloat("Camouflage", 1 - timer / transitionTime);
            timer += Time.deltaTime;
            yield return null;
        }
        isCamouflaging = false;

        if (this.playerController.stats.isCamouflaged)
        {
            this.playerController.doNormalMovement = false;
            this.playerController.lockRotation = true;
        }
        else
        {
            this.playerController.doNormalMovement = true;
            this.playerController.lockRotation = false;
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
