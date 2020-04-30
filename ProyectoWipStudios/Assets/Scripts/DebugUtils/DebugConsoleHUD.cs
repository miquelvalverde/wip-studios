using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsoleHUD : MonoBehaviour
{
    [SerializeField] private GameObject parentPanel = null;
    [SerializeField] private Text debugText = null;
    private bool isShowing;
    private InputSystem controls;

    private void Awake()
    {
        controls = new InputSystem();

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

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

}
