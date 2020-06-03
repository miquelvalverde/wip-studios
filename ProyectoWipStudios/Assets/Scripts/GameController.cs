using UnityEngine;

public class GameController : MonoBehaivourWithInputs
{
    [SerializeField] private GameObject pauseMenuHUD = null;

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
    }

    public void GameReanudate()
    {
        Time.timeScale = 1;
        pauseMenuHUD.SetActive(false);
        player.EnableInputs();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
