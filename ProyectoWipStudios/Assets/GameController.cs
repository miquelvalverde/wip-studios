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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GameReanudate()
    {
        Time.timeScale = 1;
        pauseMenuHUD.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GameQuit()
    {
        Application.Quit();
    }
} 
