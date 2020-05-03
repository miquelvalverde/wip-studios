using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ProtoAI : AMonoBehaivourWithInputs
{
    private MeshRenderer meshRenderer;

    private Transform parent;
    private bool isSeeingPlayer = false;
    [SerializeField] private MeshRenderer parentMeshRenderer = null;
    [SerializeField] private Material dontSeeMaterial = null;
    [SerializeField] private Material seeMaterial = null;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float stopTime = 2;
    private float timer = 0;
    [SerializeField] private float angle = 90;
    private float remainingAngle = 0;

    private bool isStopped;

    protected override void SetControls()
    {
        controls.Debug.AIVision.performed += _ => ToggleVisionMaterial();
    }

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        parent = parentMeshRenderer.transform;
    }

    private void Update()
    {
        parentMeshRenderer.material = (isSeeingPlayer) ? seeMaterial : dontSeeMaterial;

        if (!isStopped && !isSeeingPlayer)
            timer -= Time.deltaTime;

        if (timer < 0 && !isStopped && !isSeeingPlayer)
        {
            isStopped = true;

            timer = stopTime;

            if (remainingAngle == 0)
            {
                remainingAngle = 0;
                StartCoroutine(RotateMe(new Vector3(0, angle, 0), turnSpeed));
            }
            else
            {
                StartCoroutine(RotateMe(new Vector3(0, remainingAngle, 0), turnSpeed));
                remainingAngle = 0;
            }
        }

    }

    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        var fromAngle = parent.rotation;
        var toAngle = Quaternion.Euler(parent.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            if (isSeeingPlayer)
            {
                Vector3 playerPosition = PlayerController.instance.transform.position;
                playerPosition.y = 0;

                parent.forward = -(playerPosition - new Vector3(parent.position.x, 0, parent.position.z));
                remainingAngle = 90 - (parent.eulerAngles.y % 90);
                timer = -1;
                continue;
            }

            parent.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }

        isStopped = false;
    }

    private void ToggleVisionMaterial()
    {
        meshRenderer.enabled = !meshRenderer.enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherController = other.GetComponent<PlayerController>();
        if (otherController != null && !otherController.stats.isCamouflaged)
            isSeeingPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            isSeeingPlayer = false;
    }

}
