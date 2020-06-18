using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimationHandler : MonoBehaviourPlayerGettable
{
    int layers;

    private void Awake()
    {
        layers = 1 << 8;
    }

    public void WalkStep()
    {
        if (player.stats.speed < 0.2)
            return;

        Debug.DrawRay(transform.parent.position, Vector3.down * 5, Color.green);

        if (Physics.Raycast(transform.parent.parent.position + Vector3.up, Vector3.down, out RaycastHit hit, 5, layers))
        {
            string groundTag = hit.collider.tag;

            if(groundTag == "stoneTag")
            {
                SoundManager.StepStone.start();
            }
            else if(groundTag == "grassTag")
            {
                SoundManager.StepGrass.start();
            }
            else if(groundTag == "woodTag")
            {
                SoundManager.StepWood.start();
            }
        }
    }
}

