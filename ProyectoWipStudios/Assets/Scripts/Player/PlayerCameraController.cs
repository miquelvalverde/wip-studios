using UnityEngine;
using System.Collections;

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

    private Vector3 desiredPosition;
    private Vector3 direction;
    private float distance;

    private Vector2 mouseInput;
    private bool moveBackInput;

    private bool isShaking = false;

    private void Start()
    {
        distanceToLookAt = maxDistanceToLookAt;
        isShaking = false;
    }

    protected override void SetControls()
    {
        controls.Player.Look.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += _ => mouseInput = Vector2.zero;

        controls.Player.Zoom.performed += ctx => DoZoom(ctx.ReadValue<Vector2>().y);
        controls.Player.CameraReset.performed += _ => moveBackInput = true;
    }

    public void UpdateCamera()
    {
        if (isShaking)
            return;

        float mouseX = mouseInput.x;
        float mouseY = mouseInput.y;

        desiredPosition = transform.position;
        direction = transform.forward;
        distance = Vector3.Distance(transform.position, PlayerController.instance.cameraPoint.position);

        float yaw = 0;
        float pitch = 0;

        Vector3 eulerAngles = transform.eulerAngles;
        if (!moveBackInput)
        {
            yaw = (eulerAngles.y + 180);
            pitch = eulerAngles.x;
        }
        else
        {
            moveBackInput = false;
            yaw = player.transform.eulerAngles.y + 180;
        }

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
        int scrollAmount = (amount != 0) ? -(int)Mathf.Sign(amount) : 0;

        distanceToLookAt = Mathf.Clamp(distanceToLookAt + scrollAmount, minDistanceToLookAt, maxDistanceToLookAt);
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeEnumerator(duration, magnitude));
    }

    private IEnumerator ShakeEnumerator(float duration, float magnitude)
    {
        isShaking = true;

        Vector3 originalPos = Camera.main.transform.localPosition;

        float elapsed = .0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.position = originalPos + new Vector3(x, y, z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;

        isShaking = false;
    }

}
