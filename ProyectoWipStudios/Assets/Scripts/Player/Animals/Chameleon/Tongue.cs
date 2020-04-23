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
    private float factor;
        
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

    public void ProtractTongue(Pickable toPickable) /* METHOD TO CALL */
    {
        if(toPickable == null || isProtracting || isRetracting)
            return;

        var direction = toPickable.transform.position - tongueScaler.position;
        tongueScaler.forward = direction;
        pickableDistance = Vector3.Distance(tongueScaler.position, toPickable.transform.position);
        isProtracting = true;
        tongueScaler.gameObject.SetActive(true);
    }

    private void Update()
    {
        ///// Temporary use of old input system just for testing purposes
        if (Input.GetMouseButtonDown(0))
        {
            closestPickable = GetClosestPickable();
            ProtractTongue(closestPickable);
        }
        /////

        UpdateTongueMovement();
    }

    private void UpdateTongueMovement()
    {
        if (isProtracting)
        {
            currentTime += Time.deltaTime;
            factor = movementCurve.Evaluate(currentTime);
            tongueScaler.localScale += new Vector3(0, 0, speed * factor * Time.deltaTime);

            if (tongueScaler.localScale.z >= pickableDistance)
            {
                RetractTongue();
            }
        }
        else if (isRetracting)
        {
            currentTime += Time.deltaTime;
            factor = movementCurve.Evaluate(currentTime);
            tongueScaler.localScale -= new Vector3(0, 0, speed * factor * Time.deltaTime);
            closestPickable.transform.position = tongueEnd.position;

            if (tongueScaler.localScale.z <= 1)
            {
                Eat();
            }
        }
    }

    private void RetractTongue()
    {
        isProtracting = false;
        isRetracting = true;
    }

    private void Eat()
    {
        isRetracting = false;
        if (closestPickable != null)
        {
            Destroy(closestPickable.gameObject);
        }
        ResetTongue();
    }

    private Pickable GetClosestPickable()
    {
        var colliders = Physics.OverlapSphere(transform.position, range);
        var pickables = ExtractPickablesOnly(colliders);
        var minDistance = range;
        Pickable closestPickable = null;
        foreach (Pickable pickable in pickables)
        {
            var distance = Vector3.Distance(tongueScaler.position, pickable.transform.position);
            var direction = pickable.transform.position - tongueScaler.position;
            if (distance < minDistance && Vector3.Dot(direction, transform.forward) > 0)
            {
                closestPickable = pickable;
                minDistance = distance;
            }
        }
        return closestPickable;
    }

    private static List<Pickable> ExtractPickablesOnly(Collider[] colliders)
    {
        var pickables = new List<Pickable>();

        for (int i = 0; i < colliders.Length; i++)
        {
            var pickable = colliders[i].GetComponent<Pickable>();
            if (pickable != null)
                pickables.Add(pickable);
        }

        return pickables;
    }
}
