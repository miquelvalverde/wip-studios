using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TreeInteractable))]
public class TreeBrakeable : MonoBehaviour, IBreakable
{
    [SerializeField] private float fallAngle;
    [SerializeField] private float fallTime;    
    [SerializeField] private AnimationCurve fallCurve;
    private TreeInteractable treeInteractable;
    private float debugHeight = 10F;
    private int debugArrowCount = 5;
    private float debugArrowLength = 2F;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Break();
        }
    }

    public void Break()
    {
        if(treeInteractable.CanInteract)
        {
            treeInteractable.CanInteract = false;
            StartCoroutine(TreeFallRoutine());
        }
    }

    private void Awake()
    {
        treeInteractable = GetComponent<TreeInteractable>();
    }

    IEnumerator TreeFallRoutine()
    {
        var direction = transform.right;
        direction = Quaternion.AngleAxis(fallAngle, transform.up) * direction;
        var currentDirection = transform.up;
        float timer = 0.0f;
        while (timer <= fallTime)
        {
            transform.up = Vector3.Lerp(currentDirection, direction, fallCurve.Evaluate(timer / fallTime));
            timer += Time.deltaTime;
            yield return null;
        }
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
