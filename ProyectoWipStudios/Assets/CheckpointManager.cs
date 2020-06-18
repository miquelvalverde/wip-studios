using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    [HideInInspector] public Vector3 lastPosition;
    [HideInInspector] public Quaternion lastRotation;
    [HideInInspector] public bool mustRestartAtCheckpoint = false;
        
    public void SaveCheckpoint(Checkpoint checkpoint)
    {
        lastPosition = checkpoint.SpawnPosition;
        lastRotation = checkpoint.SpawnRotation;
        mustRestartAtCheckpoint = true;
    }


}
