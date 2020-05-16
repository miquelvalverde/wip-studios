using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMonoBehaivour : MonoBehaviour
{
    protected PlayerController player
    {
        get
        {
            return PlayerController.instance;
        }
    }
}
