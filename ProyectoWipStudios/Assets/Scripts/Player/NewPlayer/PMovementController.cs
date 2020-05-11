using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PMovementController : AMonoBehaivourWithInputs
{

    //SelfComponents
    private Rigidbody rb;

    [Header("Walk")]
    private Vector2 moveInput;
    [SerializeField] private float acceleration = .1f;
    [SerializeField] private float deceleration = .05f;
    private float speedSmoothVelocity;
    private float currentSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    protected override void SetControls()
    {
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => moveInput = Vector2.zero;
    }

    public void DoUpdate()
    {
        
    }

    public void DoFixedUpdate()
    {
        
    }

    private void CalculateNormalMovement()
    {
        Vector3 targetSpeed = player.stats.speed * moveInput;
        targetSpeed.y = rb.velocity.y;
        //currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed.mag, ref speedSmoothVelocity, acceleration);
    }

}
