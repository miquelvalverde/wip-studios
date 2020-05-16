﻿using System.Collections.Generic;
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

    public Pickable CurrentPickable { get; private set; }

    private void Awake()
    {
       ResetTongue();
    }   
        
    public void DoTongue(float dropForce = 0)
    {
        if (isProtracting || isRetracting)
            return;

        if (CurrentPickable != null)
        {
            Drop(dropForce);
            return;
        }

        var closestPickable = GetClosestPickable();
        if (closestPickable == null)
        {
            ProtractTongue(transform.position + transform.forward * range);
            return;
        }

        CurrentPickable = closestPickable;
        ProtractTongue(CurrentPickable.transform.position);
    }
        
    public void Drop(float force)
    {
        CurrentPickable?.Drop(transform.position, transform.forward, force);
        CurrentPickable = null;
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

            if(CurrentPickable != null)
                CurrentPickable.transform.position = tongueEnd.position;

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
        CurrentPickable?.Pick();
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
