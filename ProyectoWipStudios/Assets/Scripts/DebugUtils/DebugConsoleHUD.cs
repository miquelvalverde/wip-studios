using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsoleHUD : AMonoBehaivourWithInputs
{
    [SerializeField] private GameObject parentPanel = null;
    [SerializeField] private Text debugText = null;
    private bool isShowing;

    protected override void SetControls()
    {
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
