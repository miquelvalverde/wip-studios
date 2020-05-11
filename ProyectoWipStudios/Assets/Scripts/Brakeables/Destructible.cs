using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour, IBreakable
{
    [SerializeField] private GameObject destructibleVersion = null;

    public void Break()
    {
        Instantiate(destructibleVersion, transform.position, transform.rotation);
        Destroy(gameObject); 
    }
}
