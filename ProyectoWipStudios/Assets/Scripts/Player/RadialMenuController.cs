﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RadialMenuController : MonoBehaivourWithInputs
{
    [System.Serializable]
    public struct Portion
    {
        public RadialMenuPortionScriptable portion;
        public UnityEvent callback;
        public bool isLockedInitially;
    }
        
    [SerializeField] private Portion[] portions = null;
    [SerializeField] private RadialMenuPortion portionPrefabRef = null;
    [SerializeField] private float iconDistanceFromCenter = 210;
    [SerializeField] private Color unselectedPortionBackground = Color.black;
    [SerializeField] private float hoverSize = 1.6f;
    private RadialMenuPortion[] radialMenuPortions;
    private int portionCount;
    private float portionSize360;
    private float portionSize01;
    private int currentSelection;

    private bool IsHoldingToChange;
    private Vector2 pixelsMouse;
    private Vector2 deltaMouse;
    private float tolerance = 2.0F;
    private int previousSelection;
    private int previousHoverSelection = 0;    

    // Hardcoded icon rotation fixes. TODO improve solution
    private float[] iconRotation = new float[3] { 60, 180, -60 };

    protected override void SetControls()
    {
        canDisableInputs = false; // On this case its need to be always enabled.
        controls.Player.Change.performed += _ => EnableRadialMenu();
        controls.Player.Change.canceled += _ => DisableRadialMenu();
        controls.UI.MousePosition.performed += ctx => pixelsMouse = ctx.ReadValue<Vector2>();
        controls.UI.MouseDelta.performed += ctx => deltaMouse = ctx.ReadValue<Vector2>();
        controls.UI.MouseDelta.canceled += _ => deltaMouse = Vector2.zero;
        this.gameObject.SetActive(false);
    }

    public void Initializate()
    {
        portionCount = portions.Length;
        portionSize360 = 360F / portionCount;
        portionSize01 = portionSize360 / 360F;
        currentSelection = 0;
        radialMenuPortions = CreateWheel();
    }

    private void SubmitSelection()
    {
        if(radialMenuPortions[currentSelection].isLocked)
        {
            Debug.Log("This animal is locked");
            currentSelection = previousSelection;
            // TODO throw message system
        }
        else if (IsHoldingToChange && currentSelection != -1 && currentSelection != previousSelection)
        {
            SelectPortion(currentSelection);
        }
    }

    private RadialMenuPortion[] CreateWheel()
    {
        var portionInstances = new RadialMenuPortion[portionCount];
        for (int portionIndex = 0; portionIndex < portionCount; portionIndex++)
        {
            if (CheckpointManager.Instance != null && CheckpointManager.Instance.mustUnlockCertainAnimals)
            {
                portions[portionIndex].isLockedInitially = CheckpointManager.Instance.unlockedAnimals[portionIndex];
            }

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
        portion.iconRect.rotation = Quaternion.Euler(0, 0, iconRotation[index]);
        portion.iconPivot.rotation = Quaternion.Euler(0,0, -portionSize360 / 2);
        portion.background.color = unselectedPortionBackground;
        portion.background.fillAmount = portionSize01;
        portion.backgroundRect.rotation = Quaternion.Euler(0,0, -index * portionSize360);
        portion.callback = settings.callback;
        portion.animal = settings.portion.animal;
        (settings.isLockedInitially ? (Action) portion.Lock : portion.Unlock)();
        return portion;
    }

    public void UpdateRadialMenu()
    {
        if (IsHoldingToChange && !player.IsDoingSomething())
        {
            Time.timeScale = .1f;
            var mouseAngle = GetAngleFromMouseInput(pixelsMouse);

            if (deltaMouse.magnitude > tolerance)
            {
                currentSelection = (int)(mouseAngle / portionSize360);
            }

            HoverPortion(currentSelection);
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void EnableRadialMenu()
    {
        if (PlayerController.instance.IsDoingSomething())
            return;

        player.DisableSpecificController();

        previousSelection = currentSelection;
        this.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        IsHoldingToChange = true;
        SoundManager.UISelect.start();
    }

    private void DisableRadialMenu()
    {
        SubmitSelection();

        player.EnableInputs();

        this.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        IsHoldingToChange = false;
        SoundManager.UISelect.start();
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
                radialMenuPortions[portionIndex].iconRect.localScale = new Vector3(hoverSize, hoverSize, 1);
            }
            else
            {
                radialMenuPortions[portionIndex].iconRect.localScale = Vector3.one;
            }
        }

        if(previousHoverSelection != selectedPortionIndex)
        {
            SoundManager.UIHover.start();
        }
        previousHoverSelection = selectedPortionIndex;
    }

    private float GetAngleFromMouseInput(Vector3 mousePosition)
    {
        var screenHalf = new Vector3(Screen.width / 2, Screen.height / 2);
        var nonNormalizedAngle = Vector3.SignedAngle(Vector3.up, mousePosition - screenHalf, -Vector3.forward);
        var normalizedAngle = (nonNormalizedAngle + 360F) % 360F;
        return normalizedAngle;
    }

    public void Unlock(UnlockType unlockType)
    {
        if(unlockType != UnlockType.NONE)
        {
            radialMenuPortions.Where(p => p.animal.Equals(unlockType)).FirstOrDefault().Unlock();
        }
    }

    public bool[] GetUnlocksArray()
    {
        bool a = radialMenuPortions[0].isLocked;
        bool b = radialMenuPortions[1].isLocked;
        bool c = radialMenuPortions[2].isLocked;
        return new bool[3] { a, b, c };
    }
}
