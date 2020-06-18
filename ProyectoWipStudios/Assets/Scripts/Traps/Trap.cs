using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    [SerializeField] private Animation trapAnimation = null;
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
        {
            Activate(false);
            Destroy(other.gameObject);
        }
    }

    private void Activate(bool killPlayer)
    {
        SoundManager.BearTrap.start();
        isDeployed = true;
        trapAnimation.Play();
        if (killPlayer)
            PlayerController.instance.Dead();
    }

}
