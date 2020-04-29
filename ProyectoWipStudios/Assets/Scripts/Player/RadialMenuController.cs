using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RadialMenuController : MonoBehaviour
{
    [System.Serializable]
    public struct Portion
    {
        public RadialMenuPortionScriptable portion;
        public UnityEvent callback;
    }

    [SerializeField] private Portion[] portions;
    [SerializeField] private RadialMenuPortion portionPrefabRef;
    [SerializeField] private float iconDistanceFromCenter;
    [SerializeField] private Color unselectedPortionBackground;
    private RadialMenuPortion[] radialMenuPortions;
    private int portionCount;
    private float portionSize360;
    private float portionSize01;
    private int currentSelection;

    private bool IsHoldingToChange;

    public void Initializate(InputSystem controls)
    {
        controls.Player.Change.performed += _ => EnableRadialMenu();
        controls.Player.Change.canceled += _ => DisableRadialMenu();
        controls.UI.Submit.performed += _ => SubmitSelection();
        portionCount = portions.Length;
        portionSize360 = 360F / portionCount;
        portionSize01 = portionSize360 / 360F;
        currentSelection = -1;
        radialMenuPortions = CreateWheel();
        DisableRadialMenu();
    }

    private void SubmitSelection()
    {
        if (currentSelection != -1)
        {
            SelectPortion(currentSelection);
            currentSelection = -1;
        }
    }

    private RadialMenuPortion[] CreateWheel()
    {
        var portionInstances = new RadialMenuPortion[portionCount];
        for (int portionIndex = 0; portionIndex < portionCount; portionIndex++)
        {
            var portion = CreatePortion(portionIndex, portions[portionIndex]);
            portionInstances[portionIndex] = portion;
        }
        return portionInstances;
    }

    private RadialMenuPortion CreatePortion(int index,  Portion settings)
    {   
        var portion = Instantiate(portionPrefabRef, this.transform) as RadialMenuPortion;
        portion.icon.sprite = settings.portion.icon;
        portion.iconRect.localPosition = new Vector3(0, iconDistanceFromCenter, 0);
        portion.iconPivot.rotation = Quaternion.Euler(0,0, (index * -portionSize360) -portionSize360 / 2);
        portion.background.color = settings.portion.background;
        portion.background.fillAmount = portionSize01;
        portion.backgroundRect.rotation = Quaternion.Euler(0,0, index * -portionSize360);
        portion.callback = settings.callback;
        return portion;
    }

    public void UpdateRadialMenu()
    {
        if (IsHoldingToChange && !PlayerController.instance.IsDoingSomething())
        {
            Time.timeScale = .1f;
            var mouseAngle = GetAngleFromMouseInput(Input.mousePosition);
            currentSelection = (int)(mouseAngle / portionSize360);
            HoverPortion(currentSelection);
        }
        else
        {
            Time.timeScale = 1;
            currentSelection = -1;
        }
    }

    private void EnableRadialMenu()
    {
        if (PlayerController.instance.IsDoingSomething())
            return;

        this.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        IsHoldingToChange = true;
    }

    private void DisableRadialMenu()
    {
        this.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        IsHoldingToChange = false;
    }

    private void SelectPortion(int selectedPortionIndex)
    {
        radialMenuPortions[selectedPortionIndex].callback.Invoke();
    }

    private void HoverPortion(int selectedPortionIndex)
    {
        for (int portionIndex = 0; portionIndex < portionCount; portionIndex++)
        {
            if(portionIndex == selectedPortionIndex)
            {
                radialMenuPortions[portionIndex].background.color = portions[portionIndex].portion.background;
            }
            else
            {
                radialMenuPortions[portionIndex].background.color = unselectedPortionBackground;
            }
        }
    }

    private float GetAngleFromMouseInput(Vector3 mousePosition)
    {
        var screenHalf = new Vector3(Screen.width / 2, Screen.height / 2);
        var nonNormalizedAngle = Vector3.SignedAngle(Vector3.up, mousePosition - screenHalf, -Vector3.forward);
        var normalizedAngle = (nonNormalizedAngle + 360F) % 360F;
        return normalizedAngle;
    }
}
