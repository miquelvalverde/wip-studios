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
    private float tongueInitialSize = 0.1F;
        
    private void Awake()
    {
       ResetTongue();
    }   
        
    public void DoTongue()
    {
        closestPickable = GetClosestPickable();

        if (closestPickable == null || isProtracting || isRetracting)
            return;

        ProtractTongue(closestPickable);
    }

    private void ResetTongue()
    {
        tongueScaler.localPosition = Vector3.zero;
        tongueScaler.localScale = new Vector3(1, 1, tongueInitialSize);
        tongueScaler.localRotation = Quaternion.identity;
        isProtracting = false;
        isRetracting = false;
        currentTime = 0;
        tongueScaler.gameObject.SetActive(false);
        closestPickable = null;
    }

    private void ProtractTongue(Pickable toPickable)
    {
        var direction = toPickable.transform.position - tongueScaler.position;
        tongueScaler.forward = direction;
        pickableDistance = Vector3.Distance(tongueScaler.position, toPickable.transform.position);
        isProtracting = true;
        tongueScaler.gameObject.SetActive(true);
    }

    private void Update()
    {
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

            if (tongueScaler.localScale.z <= tongueInitialSize)
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
