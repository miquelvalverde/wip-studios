using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteractable : MonoBehaviour
{

    [SerializeField] private List<Transform> climbPoints = new List<Transform>();
    [SerializeField] private float radius = .5f;
    private int climbIndex = 0;

    public Vector3 GetNextPosition(Transform player)
    {
        Vector3 nextClimbPoint;

        try
        {
            nextClimbPoint = climbPoints[climbIndex].position;
        }
        catch (System.ArgumentOutOfRangeException e)
        {
            climbIndex = 0;
            throw new System.Exception();
        }

        climbIndex++;

        Vector3 direction = player.position - nextClimbPoint;
        direction.y = 0;
        direction.Normalize();

        Vector3 nextRealPoint = (climbIndex == climbPoints.Count) ? nextClimbPoint : nextClimbPoint + (direction * radius);

        return nextRealPoint;
    }

    public Vector3 GetForward(Transform player)
    {
        Vector3 newForward = climbPoints[climbIndex].position - player.position;
        newForward.y = 0;
        newForward.Normalize();
        return newForward;
    }

    public void ResetTree()
    {
        climbIndex = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(transform.position, new Vector3(radius, 20, radius));
    }

}
