using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Animator))]
public class DeadMenuHUDController : MonoBehaviour
{

    private Animator anim;

    public static DeadMenuHUDController instance;

    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);

        instance = this;

        anim = GetComponent<Animator>();
    }

    public void DisplayDeadMenu()
    {
        anim.SetBool("isShowing", true);
    }

    public void Retry()
    {
        //That will change on alpha
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
