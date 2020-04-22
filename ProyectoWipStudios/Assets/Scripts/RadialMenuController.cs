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

    private void Awake()
    {
        portionCount = portions.Length;
        portionSize360 = 360F / portionCount;
        portionSize01 = portionSize360 / 360F;
    }

    private void Start()
    {
        radialMenuPortions = CreateWheel();
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

    private void Update()
    {
        var mouseAngle = GetAngleFromMouseInput(Input.mousePosition);
        var selectedPortionIndex = (int)(mouseAngle / portionSize360);

        if(this.gameObject.activeSelf)
        {
            HoverPortion(selectedPortionIndex);

            if (Input.GetMouseButtonDown(0))
            {
                SelectPortion(selectedPortionIndex);
            }
        }
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
