using UnityEngine;

public class MyMonoBehaivour : MonoBehaviour
{

    protected PController player
    {
        get
        {
            return PController.instance;
        }
    }

}
