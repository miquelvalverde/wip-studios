using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuController : MonoBehaviour
{
    [SerializeField] private RadialMenuPortionScriptable[] portions;
    [SerializeField] private RadialMenuPortion portionPrefabRef;
    [SerializeField] private float iconDistanceFromCenter;
        
    private void Start()
    {
        CreateWheel();
    }

    private void CreateWheel()
    {
        var portionCount = portions.Length;
        var portionSize360 = 360 / portionCount;
        var portionSize01 = portionSize360 / 360.0f;
        for (int portionIndex = 0; portionIndex < portionCount; portionIndex++)
        {
            CreatePortion(portionIndex, portionSize01, portionSize360, portions[portionIndex]);
        }
    }

    private void CreatePortion(int index, float size01, float size360, RadialMenuPortionScriptable settings)
    {   
        var portion = Instantiate(portionPrefabRef, this.transform) as RadialMenuPortion;
        portion.icon.sprite = settings.icon;
        portion.iconRect.localPosition = new Vector3(0, iconDistanceFromCenter, 0);
        portion.iconPivot.rotation = Quaternion.AngleAxis(-size360 / 2, Vector3.forward);
        portion.background.color = settings.background;
        portion.background.fillAmount = size01;
        portion.backgroundRect.rotation = Quaternion.AngleAxis(index * size360, Vector3.forward);
    }
}
