using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private Rigidbody rb;
        
    private void Awake()
    {
        this.rb = GetComponent<Rigidbody>();
        this.rb.isKinematic = false;
    }

    public void Pick()
    {
        this.gameObject.SetActive(false);
        this.rb.isKinematic = true;
    }

    public void Drop(Vector3 position, Vector3 direction, float force)
    {
        this.gameObject.transform.position = position;
        this.gameObject.SetActive(true);
        this.rb.isKinematic = false;
        this.rb.AddForce(direction * force);
    }
}
