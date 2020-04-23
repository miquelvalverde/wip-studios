using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    [SerializeField] private Transform tongueScaler;
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private float speed;
    [SerializeField] private Transform tongueEnd;
    [SerializeField] private float range;
    private bool isProtracting;
    private bool isRetracting;
    private float pickableDistance;
    private float currentTime;
    private Pickable closestPickable;
        
    private void Awake()
    {
       ResetTongue();
    }   

    private void ResetTongue()
    {
        tongueScaler.localPosition = Vector3.zero;
        tongueScaler.localScale = Vector3.one;
        tongueScaler.localRotation = Quaternion.identity;
        isProtracting = false;
        isRetracting = false;
        currentTime = 0;
        tongueScaler.gameObject.SetActive(false);
        closestPickable = null;
    }

    public void ProtractTongue(Pickable toPickable)
    {
        if(toPickable == null)
            return;

        var direction = toPickable.transform.position - tongueScaler.position;
        tongueScaler.forward = direction;
        pickableDistance = Vector3.Distance(tongueScaler.position, toPickable.transform.position);
        isProtracting = true;
        tongueScaler.gameObject.SetActive(true);
    }

    private void RetractTongue()
    {
        isRetracting = true;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            closestPickable = GetClosestPickable();
            ProtractTongue(closestPickable);
        }

        currentTime += Time.deltaTime;
        float factor = movementCurve.Evaluate(currentTime);

        if (isProtracting)
        {
            tongueScaler.localScale += new Vector3(0, 0, speed * factor * Time.deltaTime);

            if(tongueScaler.localScale.z >= pickableDistance)
            {
                isProtracting = false;
                currentTime = 0;
                RetractTongue();
            }
        }
        else if (isRetracting)
        {
            tongueScaler.localScale -= new Vector3(0, 0, speed * factor * Time.deltaTime);
            closestPickable.transform.position = tongueEnd.position;
            if (tongueScaler.localScale.z <= 1)
            {
                isRetracting = false;
                if(closestPickable != null)
                {
                    Destroy(closestPickable.gameObject);
                }
                ResetTongue();
            }
        }
    }

    private Pickable GetClosestPickable()
    {
        float minDistance = float.MaxValue;
        Pickable closestPickable = null;
        foreach (Pickable pickable in GameObject.FindObjectsOfType<Pickable>())
        {
            var distance = Vector3.Distance(tongueScaler.position, pickable.transform.position);
            var direction = pickable.transform.position - tongueScaler.position;
            if (distance < minDistance && IsValidPickable(distance, direction))
            {
                closestPickable = pickable;
                minDistance = distance;
            }
        }
        return closestPickable;
    }

    public bool IsValidPickable(float distance, Vector3 direction)
    {
        return distance < range && Vector3.Dot(direction, transform.forward) > 0;
    }
}
