using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteractable : MonoBehaviour
{

    [SerializeField] private List<Transform> climbPoints = new List<Transform>();
    [SerializeField] private float radius = .5f;
    [SerializeField] private bool maxClimbForward = false;
    private int climbIndex = 0;

    public Vector3 GetNextPosition(Transform player)
    {
        Vector3 nextClimbPoint;

        try
        {
            nextClimbPoint = climbPoints[climbIndex].position;
        }
        catch (System.ArgumentOutOfRangeException)
        {
            ResetTree();
            throw new CannotClimbException();
        }

        climbIndex++;

        if (climbIndex == climbPoints.Count)
            return nextClimbPoint;

        Vector3 direction = Vector3.zero;

        if (maxClimbForward)
        {
            Vector3 forwardPosition = nextClimbPoint + (transform.forward * radius);
            Vector3 backwardPosition = nextClimbPoint + (-transform.forward * radius);
            return (Vector3.Distance(player.position, forwardPosition) < Vector3.Distance(player.position, backwardPosition)) ? forwardPosition : backwardPosition;
        }
        else
        {
            Vector3 rightPosition = nextClimbPoint + (transform.right * radius);
            Vector3 leftPosition = nextClimbPoint + (-transform.right * radius);
            return (Vector3.Distance(player.position, rightPosition) < Vector3.Distance(player.position, leftPosition)) ? rightPosition : leftPosition;
        }
    }

    public Vector3 GetForward(Transform player)
    {
        Vector3 nextClimbPoint = climbPoints[climbIndex].position;

        if (maxClimbForward)
        {
            Vector3 forwardPosition = nextClimbPoint + (transform.forward * radius);
            Vector3 backwardPosition = nextClimbPoint + (-transform.forward * radius);
            return (Vector3.Distance(player.position, forwardPosition) < Vector3.Distance(player.position, backwardPosition)) ? -transform.forward : transform.forward;
        }
        else
        {
            Vector3 rightPosition = nextClimbPoint + (transform.right * radius);
            Vector3 leftPosition = nextClimbPoint + (-transform.right * radius);
            return (Vector3.Distance(player.position, rightPosition) < Vector3.Distance(player.position, leftPosition)) ? -transform.right : transform.right;
        }
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
