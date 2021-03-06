﻿using UnityEngine;

public class BoarController : PlayerSpecificController
{
    [SerializeField] private float runSpeed = 10;
    [SerializeField] private float timeRunning = 2;
    [SerializeField] private float stunTime = .5f;
    [SerializeField] private ParticleSystem stuntParticles = null;

    [Space]
    [Header("Crash Checker")]
    [SerializeField] private Vector3 checkerOffset = Vector3.zero;
    [SerializeField] private Vector3 checkerDimensions = Vector3.zero;
    [SerializeField] private LayerMask whatIsObstacle = 0;

    public override void Initializate()
    {
        this.controls.Player.Run.performed += _ => Run();
        MyAnimalType = Type.Boar;
    }

    public override void UpdateSpecificAction()
    {
        if (PlayerController.instance.stats.isRunning && this.HasCrashed())
            ExitRun();
    }

    private void Run()
    {
        if (PlayerController.instance.specificController.GetType() != typeof(BoarController))
            return;

        this.playerController.useMovementInputs = false;
        this.playerController.lockRotation = true;
        this.playerController.stats.isRunning = true;

        this.playerController.ChangeSpeed(runSpeed);

        SoundManager.BoarCharge.start();
        Invoke("ExitRun", timeRunning);
    }

    private void ExitRun()
    {
        CancelInvoke("ExitRun");

        this.playerController.ResetSpeed();
        SoundManager.BoarCharge.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        this.stuntParticles.Play();

        this.playerController.stats.isRunning = false;
        player.stats.isStunded = true;
        Invoke("ReturnControl", stunTime);
    }

    private void ReturnControl()
    {
        this.playerController.lockRotation = false;
        this.playerController.useMovementInputs = true;
        player.stats.isStunded = false;
        this.stuntParticles.Stop();
    }

    private bool HasCrashed()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + ((transform.forward * checkerOffset.z) + (transform.right * checkerOffset.x) + (transform.up * checkerOffset.y))
            , checkerDimensions/2, transform.rotation, whatIsObstacle);

        for(int i=0; i<colliders.Length; i++)
        {
            if (colliders[i].GetComponent<IBreakable>() != null)
                colliders[i].GetComponent<IBreakable>().Break();


            if (colliders[i].tag != "PostPo")
            {
                SoundManager.BoarHit.start();
                player.cameraController.Shake(.1f, .1f);
                return true;
            }
        }

        

        return false;
    }

    public override string ToString()
    {
        return "Boar";
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position + ((transform.forward * checkerOffset.z) + (transform.right * checkerOffset.x) + (transform.up * checkerOffset.y)), transform.rotation, checkerDimensions);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }

    public override bool CheckIfCanChange(Type to)
    {
        return true;
    }
}
