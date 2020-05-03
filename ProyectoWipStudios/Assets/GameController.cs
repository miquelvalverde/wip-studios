using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : AMonoBehaivourWithInputs
{
    [SerializeField] private GameObject pauseMenuHUD;

    void Start()
    {
        this.controls.UI.Pause.performed += _ => GamePause();
    }   

    private void GamePause()
    {
        Time.timeScale = 0;
        pauseMenuHUD.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
            
    public void GameReanudate()
    {
        Time.timeScale = 1;
        pauseMenuHUD.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
