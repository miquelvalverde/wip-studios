using System.Collections;
using UnityEngine;

public class TreeBrakeable : MonoBehaviour, IBreakable
{
    [SerializeField] private float fallAngle = -272.28f;
    [SerializeField] private float fallTime = 4;    
    [SerializeField] private AnimationCurve fallCurve = null;
    private float debugHeight = 10F;
    private int debugArrowCount = 5;
    private float debugArrowLength = 2F;

    private bool wasFall;

    public void Break()
    {
        if (wasFall)
            return;

        wasFall = true;
        StartCoroutine(TreeFallRoutine());
    }

    IEnumerator TreeFallRoutine()
    {
        SoundManager.TreeFall.start();
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
