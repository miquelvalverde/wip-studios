using UnityEngine;

public class TreeAcornSpawner : MonoBehaviour, IBreakable
{

    [SerializeField] private GameObject acornPrefab = null;
    [SerializeField] private Transform spawnPoint = null;

    public void Break()
    {
        SpawnAcorn();
    }

    private void SpawnAcorn()
    {
        Instantiate(acornPrefab, spawnPoint.position, Quaternion.identity);
    }

}
