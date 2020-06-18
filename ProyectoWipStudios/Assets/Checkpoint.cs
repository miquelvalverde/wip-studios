using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;

    public Vector3 SpawnPosition
    {
        get => spawnPosition.position;
    }

    public Quaternion SpawnRotation
    {
        get => spawnPosition.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name != "Player")
            return;

        CheckpointManager.Instance.SaveCheckpoint(this);
    }
}
