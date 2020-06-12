using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    [SerializeField] private Transform tongueScaler = null;
    [SerializeField] private AnimationCurve movementCurve = null;
    [SerializeField] private float speed = 30;
    [SerializeField] private Transform tongueEnd = null;
    [SerializeField] private float range = 5;
    private bool isProtracting;
    private bool isRetracting;
    private float pickableDistance;
    private float currentTime;
    private float factor;
    private const float INITIAL_SIZE = 0.1F;

    public Aimable CurrentAimable { get; private set; }

    private void Awake()
    {
       ResetTongue();
    }   
        
    public void DoTongue(float dropForce = 0)
    {
        if (isProtracting || isRetracting)
            return;

        if (CurrentAimable != null)
        {
            Drop(dropForce);
            return;
        }

        var closestAimable = GetClosestAimable();
        if (closestAimable == null)
        {
            ProtractTongue(transform.position + transform.forward * range);
            return;
        }

        CurrentAimable = closestAimable;
        ProtractTongue(CurrentAimable.transform.position);
    }
        
    public void Drop(float force)
    {
        if (CurrentAimable != null && CurrentAimable is Pickable pickable)
            pickable.Drop(transform.position, transform.forward, force);

        CurrentAimable = null;
    }

    private void ResetTongue()
    {
        tongueScaler.localPosition = Vector3.zero;
        tongueScaler.localScale = new Vector3(1, 1, INITIAL_SIZE);
        tongueScaler.localRotation = Quaternion.identity;
        isProtracting = false;
        isRetracting = false;
        currentTime = 0;
        tongueScaler.gameObject.SetActive(false);
        PlayerController.instance.stats.isTongue = false;
    }
        
    private void ProtractTongue(Vector3 toPosition)
    {
        var direction = toPosition - tongueScaler.position;
        tongueScaler.forward = direction;
        pickableDistance = Vector3.Distance(tongueScaler.position, toPosition);
        isProtracting = true;
        tongueScaler.gameObject.SetActive(true);
        PlayerController.instance.stats.isTongue = true;
        SoundManager.ChameleonTongue.start();
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
                Press();
                RetractTongue();
            }
        }
        else if (isRetracting)
        {
            currentTime += Time.deltaTime;
            factor = movementCurve.Evaluate(currentTime);
            tongueScaler.localScale -= new Vector3(0, 0, speed * factor * Time.deltaTime);

            if(CurrentAimable != null && CurrentAimable is Pickable)
                CurrentAimable.transform.position = tongueEnd.position;

            if (tongueScaler.localScale.z <= INITIAL_SIZE)
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
        if (CurrentAimable != null && CurrentAimable is Pickable pickable)
            pickable.Pick();

        ResetTongue();
    }
        
    private void Press()
    {
        if (CurrentAimable != null && CurrentAimable is Pressable pressable)
            pressable.callback.Invoke();
    }

    private Aimable GetClosestAimable()
    {
        var colliders = Physics.OverlapSphere(transform.position, range);
        var aimables = ExtractAimablesOnly(colliders);
        var minDistance = range;
        Aimable closestAimable = null;
        foreach (Aimable aimable in aimables)
        {
            var distance = Vector3.Distance(tongueScaler.position, aimable.transform.position);
            var direction = aimable.transform.position - tongueScaler.position;
            if (distance < minDistance && Vector3.Dot(direction, transform.forward) > 0)
            {
                closestAimable = aimable;
                minDistance = distance;
            }
        }
        return closestAimable;
    }

    private static List<Aimable> ExtractAimablesOnly(Collider[] colliders)
    {
        var aimables = new List<Aimable>();

        for (int i = 0; i < colliders.Length; i++)
        {
            var aimable = colliders[i].GetComponent<Aimable>();
            if (aimable != null)
                aimables.Add(aimable);
        }

        return aimables;
    }
}
