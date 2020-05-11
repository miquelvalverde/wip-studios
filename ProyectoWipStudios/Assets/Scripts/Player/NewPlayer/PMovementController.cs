using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PMovementController : AMonoBehaivourWithInputs
{

    //SelfComponents
    private Rigidbody rb;

    [Header("Walk")]
    private Vector2 moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    public void DoUpdate()
    {
        
    }

    public void DoFixedUpdate()
    {
        
    }

}
