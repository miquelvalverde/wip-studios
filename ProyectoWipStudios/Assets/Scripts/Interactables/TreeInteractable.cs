﻿using System.Collections.Generic;
using UnityEngine;

public class TreeInteractable : MonoBehaviour
{

    [SerializeField] private Transform parentOfClimbPoints = null;
    private List<Transform> climbPoints = new List<Transform>();
    [SerializeField] private float radius = .5f;
    [SerializeField] private bool maxClimbForward = false;
    private int climbIndex = 0;

    [HideInInspector] public bool IsLastPoint
    {
        get
        {
            return climbIndex >= climbPoints.Count;
        }
    }

    private struct CustomPoint
    {
        public CustomPoint(Vector3 point, Vector3 direction)
        {
            this.point = point;
            this.direction = direction;
        }

        public Vector3 point;
        public Vector3 direction;
    }

    private void Awake()
    {
        foreach (Transform child in parentOfClimbPoints)
            climbPoints.Add(child);
    }

    public Vector3 GetNextPosition(Transform player)
    {
        if(climbIndex > climbPoints.Count-1)
        {
            ResetTree();
            throw new CannotClimbException();
        }

        Vector3 nextClimbPoint;
        nextClimbPoint = climbPoints[climbIndex].position;

        climbIndex++;

        if (climbIndex == climbPoints.Count)
            return nextClimbPoint;

        return GetNearestClimbPointToDirection(player.position, nextClimbPoint, (maxClimbForward) ? transform.forward : transform.right).point;
    }

    public Vector3 GetForward(Transform player)
    {
        Vector3 nextClimbPoint = climbPoints[climbIndex].position;

        return GetNearestClimbPointToDirection(player.position, nextClimbPoint, (maxClimbForward) ? transform.forward : transform.right).direction;
    }

    private CustomPoint GetNearestClimbPointToDirection(Vector3 playerPoint, Vector3 climbPoint, Vector3 direction)
    {
        Vector3 pointA = climbPoint + (direction * radius);
        Vector3 pointB = climbPoint + (-direction * radius);

        float distanceToA = Vector3.Distance(playerPoint, pointA);
        float distanceToB = Vector3.Distance(playerPoint, pointB);

        if (distanceToA < distanceToB)
            return new CustomPoint(pointA, -direction);
        else
            return new CustomPoint(pointB, direction);
    }

    public Vector3 GetFirstPoint()
    {
        return climbPoints[0].position;
    }

    public void ResetTree()
    {
        climbIndex = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;


        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(Vector3.up * 5, new Vector3(radius, 10, radius));

        if (maxClimbForward)
            Gizmos.DrawCube(Vector3.up * 5, new Vector3(.1f, 9.5f, 1));
        else
            Gizmos.DrawCube(Vector3.up * 5, new Vector3(1, 9.5f, .1f));
    }

}
