using UnityEngine;
using UnityEditor;

public abstract class WeaponAbilityLogic : ScriptableObject
{
    [field: SerializeField, Min(0.01f)]
    public float Width { get; private set; } = 1f;

    [field: SerializeField, Min(0.01f)]
    [field: Tooltip("Length variable won't matter if Width is bigger than Length")]
    public float Length { get; private set; } = 3f;

    [field: SerializeField]
    public float ForwardOffset { get; private set; } = 0.4f;

    [field: SerializeField, Range(0f, 360f)]
    public float AngleOffset { get; private set; }

    [field: SerializeField, Min(1f)]
    public float Damage { get; private set; } = 1f;

    [field: SerializeField, Min(0f)]
    public float AttackDelay { get; private set; } = 0.5f;

    [field: SerializeField, Min(0f)]
    public float AttackCooldown { get; private set; } = 1f;

    [SerializeField]
    private GameObject attackEffectPrefab;

    [SerializeField]
    private float attackEffectDuration = 0.1f;

    [SerializeField]
    private float effectOffset;
    protected float attackerAngle; // currently for Overlap

    protected Vector2 CalculatePoint(Transform transform, float offset)
    {
        Vector2 rotatedOffset = transform.right * offset; // rotation based on weapon's rotation
        Vector2 circlePoint = (Vector2)transform.position + rotatedOffset;
        return circlePoint;
    }

    private void CheckForHits(Collider2D[] hitColliders)
    {
        foreach (Collider2D other in hitColliders)
        {
            // Check if the object implements IDamageable interface
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(Damage);
                HitDebugMessage(other.gameObject, Damage, this);
            }
        }
    }

    // Refactored debug message for all weapon abilities
    private void HitDebugMessage(GameObject target, float damage, WeaponAbilityLogic ability)
    {
        Debug.Log($"{target} was successfully damaged for {damage} using {ability}.");
    }

    public void UseAbility(Transform attackPosition, LayerMask ignoredLayers)
    {
        Collider2D[] detectedTargets = DetectHitTargets(attackPosition, ignoredLayers);
        CheckForHits(detectedTargets);
        if (attackEffectPrefab != null)
        {
            Vector2 center = CalculatePoint(attackPosition, ForwardOffset + effectOffset);
            Quaternion effectRotation = attackPosition.rotation * Quaternion.Euler(0, 0, -90);
            GameObject effect = Instantiate(attackEffectPrefab, center, effectRotation);
            Destroy(effect, attackEffectDuration);
        }
    }
    protected virtual Collider2D[] DetectHitTargets(Transform attackPosition, LayerMask ignoredLayers)
    {
        Vector2 point = CalculatePoint(attackPosition, ForwardOffset);
        attackerAngle = attackPosition.eulerAngles.z + AngleOffset;
        CapsuleDirection2D direction = Width > Length ? CapsuleDirection2D.Vertical : CapsuleDirection2D.Horizontal;
        return Physics2D.OverlapCapsuleAll(
            point, 
            new Vector2(Width, Length), 
            direction,
            attackerAngle,
            ~ignoredLayers);
    }

#if UNITY_EDITOR
    public void DrawAttackZone(Transform point)
    {
        Handles.color = Color.green;
        // Get capsule center
        Vector2 center = CalculatePoint(point, ForwardOffset);
        // Halved because we are creating half circles here
        float radius = Width > Length ? Length * 0.5f : Width * 0.5f;
        float height = Length > Width ? Length : Width;
        // distance between the two semicircles
        float bodyLength = height - (2f * radius);
        // Make offsets based on direction
        Vector2 bodyOffset = Vector2.up;
        bodyOffset *= bodyLength * 0.5f;
        float direction = Length > Width ? 90f : 0f;
        // Set up rotation based on Character rotation + any offset
        Quaternion rotation = Quaternion.Euler(0f, 0f, point.eulerAngles.z + AngleOffset + direction);
        Vector2 rotatedOffset = rotation * bodyOffset;
        // Calculate top and bottom centers
        Vector2 topCenter = center + rotatedOffset;
        Vector2 bottomCenter = center - rotatedOffset;
        // Cache for arc drawing
        Vector3 forward = Vector3.forward;
        Vector3 right = rotation * Vector3.right;
        // Draw top semicircle
        Handles.DrawWireArc(topCenter, forward, right, 180f, radius);
        // Draw bottom semicircle
        Handles.DrawWireArc(bottomCenter, forward, -right, 180f, radius);
        // Draw the two straight lines connecting the arcs
        Vector2 sideDir = rotation * Vector2.right * radius;
        Handles.DrawLine(topCenter + sideDir, bottomCenter + sideDir);
        Handles.DrawLine(topCenter - sideDir, bottomCenter - sideDir);
    }
#endif
}
