using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(CircleCollider2D))]
public class FOVCheckerBase : MonoBehaviour
{
    [SerializeField]
    private FOVSettings fovSettings;
    private Transform targetFound;

    private WaitForSeconds wait;

    private void Start()
    {
        if (fovSettings == null)
        {
            Debug.LogWarning($"FOVSettings are missing for FOV component: {this}");
            enabled = false;
            return;
        }

        if (!TryGetComponent<CircleCollider2D>(out var collider))
        {
            Debug.LogError($"{this} is missing a circle collider");
        }

        collider.radius = fovSettings.Radius;

        wait = new(fovSettings.SearchFrequency);
    }
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTarget(other))
        {
            StartCoroutine(FOVRoutine(other.transform));
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (IsTarget(other))
        {
            StopCoroutine(FOVRoutine(other.transform));
        }
    }

    private bool IsTarget(Collider2D other)
    {
        // Checks if other's layer(s) are the targeted layer mask with an & bit operation
        return (fovSettings.TargetMask & (1 << other.gameObject.layer)) != 0;
    }

    public IEnumerator FOVRoutine(Transform target)
    {
        while (target != null)
        {
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            targetFound = IsTargetInSight(directionToTarget, target) ? target : null;

            yield return wait;
        }
    }

    private bool IsTargetInSight(Vector2 directionToTarget, Transform targetTransform)
    {
        // Check if target within the assigned angle
        if (!(Vector2.Angle(transform.right, directionToTarget) < fovSettings.Angle * 0.5)) return false;

        float distanceToTarget = Vector2.Distance(transform.position, targetTransform.position);
        // Check if the target is still within radius
        if (distanceToTarget > fovSettings.Radius) return false;

        // Check if target is obstructed by objects with the assigned layer mask
        RaycastHit2D hit = Physics2D.Raycast
        (
            transform.position,
            directionToTarget,
            distanceToTarget,
            fovSettings.BlockingMask
        );
        return hit.collider == null;
    }

    //***************************************** Gizmo logic *****************************************
#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (fovSettings == null || !fovSettings.DisplayGizmos) return;
        
        Handles.color = fovSettings.RadiusColor;
        Handles.DrawWireDisc(transform.position, transform.forward, fovSettings.Radius);
        DrawViewAngle(-1);
        DrawViewAngle(1);
        if (targetFound != null)
        {
            Handles.color = fovSettings.InSightColor;
            Handles.DrawLine(transform.position, targetFound.position);
        }
    }

    private void DrawViewAngle(float angleModifier)
    {
        Vector3 viewAngle = DirectionFromAngle(transform.rotation, angleModifier * fovSettings.Angle * 0.5f);
        Handles.color = fovSettings.AnglesColor;
        Handles.DrawLine(transform.position, transform.position + (viewAngle * fovSettings.Radius));
    }

    private static Vector3 DirectionFromAngle(Quaternion rotation, float angleInDegrees)
    {
        Vector3 direction = rotation * Vector3.right;
        direction = Quaternion.Euler(0, 0, angleInDegrees) * direction;
        return direction;
    }
#endif
}
