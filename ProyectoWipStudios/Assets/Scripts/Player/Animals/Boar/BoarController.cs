using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarController : PlayerSpecificController
{
    public override void Initializate(InputSystem controls)
    {
        controls.Player.Run.performed += _ => Run();
    }

    public override void UpdateSpecificAction()
    {
        
    }

    private void Run()
    {
        Debug.Log("Run");
    }
}
