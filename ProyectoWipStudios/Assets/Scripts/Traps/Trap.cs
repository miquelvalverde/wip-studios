using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    [SerializeField] private MeshRenderer trapMeshRenderer = null;
    [SerializeField] private Material trapActivatedMaterial = null;
    private bool isDeployed;

    private void Start()
    {
        isDeployed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDeployed)
            return;

        if (other.GetComponent<PlayerController>())
            Activate(true);

        if (other.GetComponent<Aimable>())
            Activate(false);
    }

    private void Activate(bool killPlayer)
    {
        isDeployed = true;
        trapMeshRenderer.material = trapActivatedMaterial;
        if(killPlayer)
            PlayerController.instance.Dead();
    }

}
