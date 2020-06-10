using System.Collections;
using UnityEngine;

public class ChameleonController : PlayerSpecificController
{
    [SerializeField] private Tongue tongue = null;
    [SerializeField] private float dropForce = 200;
    [SerializeField] ChameleonHUDController chameleonHUD = null;
    [SerializeField] private Material[] materials = null;
    [SerializeField] private float transitionTime = 1.5F;
    private ChameleonHUDController hudInstance;
    private bool isCamouflaging = false;
        
    public override void Initializate()
    {
        this.controls.Player.Tongue.performed += _ => TonguePick();
        this.controls.Player.Camouflage.performed += _ => Camouflage();
        hudInstance = Instantiate(chameleonHUD);
        foreach (Material material in materials)
            material.SetFloat("Camouflage", 1);
        MyAnimalType = Type.Chameleon;
    }   

    private void TonguePick()
    {
        if (!PlayerController.instance.stats.isCamouflaged)
            tongue.DoTongue(dropForce);

        hudInstance.ShowPickable(tongue.CurrentAimable is Pickable pickable ? pickable : null);
    }

    private void Camouflage()
    {
        if (!isCamouflaging)
            StartCoroutine(CamouflageRoutine(this.playerController.stats.isCamouflaged));
    }

    IEnumerator CamouflageRoutine(bool reverse)
    {
        this.playerController.stats.isCamouflaged = !this.playerController.stats.isCamouflaged;

        if (this.playerController.stats.isCamouflaged)
        {
            this.playerController.doNormalMovement = false;
            this.playerController.lockRotation = true;
            SoundManager.ChameleonCamouflage.start();
        }
        else
        {
            this.playerController.doNormalMovement = true;
            this.playerController.lockRotation = false;
            SoundManager.ChameleonDecamouflage.start();
        }

        // dissolve
        isCamouflaging = true;
        float timer = 0.0f;
        while (timer <= transitionTime)
        {
            if (reverse)
            {
                foreach (Material material in materials)
                    material.SetFloat("Camouflage", timer / transitionTime);
            }
            else
            {
                foreach (Material material in materials)
                    material.SetFloat("Camouflage", 1 - timer / transitionTime);
            }
            timer += Time.deltaTime;
            yield return null;
        }
        isCamouflaging = false;
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

    public override bool CheckIfCanChange(Type to)
    {

        if (to == Type.Boar)
            return this.CheckUp(.15f);

        return true;
    }
}
