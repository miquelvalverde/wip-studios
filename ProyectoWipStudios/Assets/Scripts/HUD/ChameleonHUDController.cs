using UnityEngine;
using UnityEngine.UI;

public class ChameleonHUDController : MonoBehaviour
{
    [SerializeField] private Image imageIcon = null;

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
