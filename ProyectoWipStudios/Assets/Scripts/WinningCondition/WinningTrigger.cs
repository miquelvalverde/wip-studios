using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningTrigger : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

}
