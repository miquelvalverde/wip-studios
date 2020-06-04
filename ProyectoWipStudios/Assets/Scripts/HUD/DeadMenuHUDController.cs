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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        //That will change on alpha
        SoundManager.Music2.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
