using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaivourWithInputs
{
    [SerializeField] private GameObject pauseMenuHUD = null;

    public static GameController instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        this.controls.UI.Pause.performed += _ => GamePause();

        GameReanudate();
    }

    private void GamePause()
    {
        Time.timeScale = 0;
        pauseMenuHUD.SetActive(true);
        player.DisableInputs();
        player.DisableCameraControls();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SoundTriggerOnPointerClick();
        SoundManager.Music2.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void GameReanudate()
    {
        Time.timeScale = 1;
        pauseMenuHUD.SetActive(false);
        player.EnableInputs();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SoundManager.Music2.start();
    }

    public void GameWin()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene(1);
    }
        
    public void GameQuit()
    {
        Application.Quit();
    }

    public void SoundTriggerOnPointerEnter()
    {
        SoundManager.UIHover.start();           
    }

    public void SoundTriggerOnPointerClick()
    {
        SoundManager.UISelect.start();
    }
} 
