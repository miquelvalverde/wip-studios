using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChameleonHUDController : MonoBehaviour
{
    [SerializeField] private Image imageIcon;

    public void ShowPickable(Pickable pickable)
    {
        if(pickable == null)
        {
            imageIcon.enabled = false;
            imageIcon.sprite = null;
        }
        else
        {
            imageIcon.enabled = true;
            imageIcon.sprite = pickable.icon;
        }
    }
}
