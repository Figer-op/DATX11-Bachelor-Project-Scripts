using UnityEngine;

[CreateAssetMenu(fileName = "RangedShooting", menuName = "WeaponAbilities/Ranged/RangedShooting")]
public class RangedShooting : WeaponAbilityLogic
{
    // TODO refactor so that ranged shooting does not need this since it does not use it.
    protected override Collider2D[] DetectHitTargets(Transform attackPosition, LayerMask ignoredLayers)
    {
        Vector2 circlePoint = CalculatePoint(attackPosition, ForwardOffset);
        return Physics2D.OverlapBoxAll(circlePoint, new Vector2(Width, Length), ~ignoredLayers);
    }
}
