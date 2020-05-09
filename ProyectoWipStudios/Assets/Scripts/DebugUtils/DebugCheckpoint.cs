using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugCheckpoint : AMonoBehaivourWithInputs
{

    [SerializeField] private Transform checkPointOne = null;
    [SerializeField] private Transform checkPointTwo = null;
    [SerializeField] private Transform checkPointThree = null;
    [SerializeField] private Transform checkPointFour = null;
    [SerializeField] private Transform checkPointFive = null;
    [SerializeField] private Transform checkPointSix = null;
    [SerializeField] private Transform checkPointSeven = null;
    [SerializeField] private Transform checkPointEight = null;
    [SerializeField] private Transform checkPointNine = null;
    [SerializeField] private Transform checkPointZero = null;

    protected override void SetControls()
    {
        controls.Debug.Checkpoints.performed += ctx => TeleportToCheckpoint(ctx.control.displayName);
    }


    private void TeleportToCheckpoint(string keyName)
    {
        switch (keyName)
        {
            case "0":
                PlayerController.instance.TeleportTo(checkPointZero);
                break;
            case "1":
                PlayerController.instance.TeleportTo(checkPointOne);
                break;
            case "2":
                PlayerController.instance.TeleportTo(checkPointTwo);
                break;
            case "3":
                PlayerController.instance.TeleportTo(checkPointThree);
                break;
            case "4":
                PlayerController.instance.TeleportTo(checkPointFour);
                break;
            case "5":
                PlayerController.instance.TeleportTo(checkPointFive);
                break;
            case "6":
                PlayerController.instance.TeleportTo(checkPointSix);
                break;
            case "7":
                PlayerController.instance.TeleportTo(checkPointSeven);
                break;
            case "8":
                PlayerController.instance.TeleportTo(checkPointEight);
                break;
            case "9":
                PlayerController.instance.TeleportTo(checkPointNine);
                break;
        }
    }

}
