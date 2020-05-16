using UnityEngine;

public class MonoBehaviourPlayerGettable : MonoBehaviour
{
    protected PlayerController player
    {
        get
        {
            return PlayerController.instance;
        }
    }
}
