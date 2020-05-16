using UnityEngine;

public class PlayerCameraController : MonoBehaivourWithInputs
{
    [SerializeField] private float maxDistanceToLookAt = 5;
    [SerializeField] private float minDistanceToLookAt = 1;
    private float distanceToLookAt = 0;

    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private float offsetOnCollision = .5f;
    [Space]
    [SerializeField] private float sensitivity = 20;
    [SerializeField] private float minPitch = -50;
    [SerializeField] private float maxPitch = 80;

    [HideInInspector] private Vector3 desiredPosition;
    [HideInInspector] private Vector3 direction;
    [HideInInspector] private float distance;

    [HideInInspector] private Vector2 mouseInput;

    private void Start()
    {
        distanceToLookAt = maxDistanceToLookAt;
    }

    protected override void SetControls()
    {
        controls.Player.Look.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += _ => mouseInput = Vector2.zero;
        controls.Player.Zoom.performed += ctx => DoZoom(ctx.ReadValue<Vector2>().y);
    }

    public void UpdateCamera()
    {
        float mouseX = mouseInput.x;
        float mouseY = mouseInput.y;

        desiredPosition = transform.position;
        direction = transform.forward;
        distance = Vector3.Distance(transform.position, PlayerController.instance.cameraPoint.position);

        float yaw = 0;
        float pitch = 0;

        Vector3 eulerAngles = transform.eulerAngles;
        yaw = (eulerAngles.y + 180);
        pitch = eulerAngles.x;

        CalculatePosition(mouseX, mouseY, yaw, pitch, PlayerController.instance.cameraPoint.position);

        transform.forward = direction;
        transform.position =  desiredPosition;

    }

    private void CalculatePosition(float mouseX, float mouseY, float yaw, float pitch, Vector3 playerPosition)
    {
        yaw += sensitivity * mouseX * Time.deltaTime;
        yaw *= Mathf.Deg2Rad;

        if (pitch > 180f)
            pitch -= 360;

        pitch += sensitivity * (-mouseY) * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        pitch *= Mathf.Deg2Rad;

        desiredPosition = playerPosition + new Vector3(Mathf.Sin(yaw) * Mathf.Cos(pitch) * distance, Mathf.Sin(pitch) * distance, Mathf.Cos(yaw) * Mathf.Cos(pitch) * distance);

        direction = playerPosition - desiredPosition;

        direction.Normalize();

        distance = distanceToLookAt;
        desiredPosition = playerPosition - direction * distanceToLookAt;

        if (distance < minDistanceToLookAt)
        {
            distance = minDistanceToLookAt;
            desiredPosition = playerPosition - direction * minDistanceToLookAt;
        }

        RaycastHit hit;

        Ray ray = new Ray(playerPosition, -direction);
        if (Physics.Raycast(ray, out hit, distance, raycastLayerMask.value))
        {
            desiredPosition = hit.point + direction * offsetOnCollision;
        }

    }

    private void DoZoom(float amount)
    {
        int scrollAmount = (amount != 0) ? (int)Mathf.Sign(amount) : 0;

        distanceToLookAt = Mathf.Clamp(distanceToLookAt + scrollAmount, minDistanceToLookAt, maxDistanceToLookAt);
    }

}
