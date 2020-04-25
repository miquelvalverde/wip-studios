using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsoleHUD : MonoBehaviour
{
    [SerializeField] private GameObject parentPanel;
    [SerializeField] private Text debugText;
    private bool isShowing;

    private void Start()
    {
        InputSystem controls = PlayerController.instance.controls;

        controls.Player.Debug.performed += _ => ToggleDebugWindow();
    }

    private void Update()
    {
        if (!isShowing)
        {
            parentPanel.SetActive(false);
            return;
        }

        parentPanel.SetActive(true);

        debugText.text = PlayerController.instance.stats.ToString();

    }
    
    private void ToggleDebugWindow()
    {
        isShowing = !isShowing;
    }

}
