using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TreeInteractable))]
public class TreeBrakeable : MonoBehaviour, IBreakable
{
    [SerializeField] private float fallAngle;
    private TreeInteractable treeInteractable;
    private float debugHeight = 10F;
    private int debugArrowCount = 5;
    private float debugArrowLength = 2F;
    private Quaternion fallRotation;

    public void Break()
    {
        treeInteractable.CanInteract = false;
    }

    private void Awake()
    {
        treeInteractable = GetComponent<TreeInteractable>();
    }

    private void OnDrawGizmos()
    {
        var currentPosition = transform.position;
        var direction = transform.right;
        direction = Quaternion.AngleAxis(fallAngle, transform.up) * direction;
        for (int i = 0; i < debugArrowCount; i++)
        {
            var deltaY = transform.up * (debugHeight / debugArrowCount);
            DrawArrow.ForGizmo(currentPosition += deltaY, direction * debugArrowLength);
        }
    }

}
