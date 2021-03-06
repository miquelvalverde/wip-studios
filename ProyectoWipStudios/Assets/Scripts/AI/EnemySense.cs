﻿using UnityEngine;

public class EnemySense : MonoBehaviourPlayerGettable
{
    [SerializeField] private Enemy self = null;

    private void OnTriggerEnter(Collider other)
    {
        if (self.stats.isSeeingPlayer)
            return;

        RaycastHit hit;
        Physics.Raycast(self.position + Vector3.up, (player.transform.position + Vector3.up) - (self.position + Vector3.up), out hit, self.chaseMaxDistance);

        if (hit.collider && hit.collider.GetComponent<PlayerController>() && !player.stats.isCamouflaged)
        {
            self.stats.isSeeingPlayer = true;
            SoundManager.ItsThere.start();
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        if (!self){
            Debug.LogWarning("Enemy of " + gameObject + " is null.", gameObject);
            return;
        }
    }

}
