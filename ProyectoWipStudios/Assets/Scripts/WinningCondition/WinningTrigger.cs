using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningTrigger : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            SceneManager.LoadScene(sceneIndex);
        }
    }

}
