using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destructibleVersion;

    private void Update()
    {
        // TODO delete. Only used to test. 
        if (Input.GetMouseButtonDown(0))
        {
            Break();
        }
    }

    public void Break()
    {
        Instantiate(destructibleVersion, transform.position, transform.rotation);
        Destroy(gameObject); 
    }
}
