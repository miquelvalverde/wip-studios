using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
    
public class RadialMenuPortion : MonoBehaviour
{
    public Image icon;
    public RectTransform iconRect;
    public RectTransform iconPivot;
    public Image background;
    public RectTransform backgroundRect;
    [SerializeField] private Image lockIcon = null;
    [HideInInspector] public bool isLocked;
    [HideInInspector] public UnityEvent callback;
    [HideInInspector] public UnlockType animal;

    public void Unlock()
    {
        isLocked = false;
        icon.color = Color.white;
        // TODO trigger some animation or unlock sound.
    }

    public void Lock()
    {
        isLocked = true;
        icon.color = Color.gray;
    }
}
