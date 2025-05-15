using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackStraightSingleProjectile", menuName = "EnemyLogic/AttackLogic/Straight Single Projectile")]
public class EnemyAttackSingleStraightProjectile : EnemyAttackBaseSO
{
    [SerializeField]
    private Rigidbody2D bulletRB;

    [SerializeField]
    private float bulletSpeed = 10f;

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        enemy.Move(Vector2.zero);
    }

    protected override void Attack(WeaponAbilityLogic currentAbility)
    {
        if (playerTransform == null)
        {
            return;
        }

        Vector2 dir = (playerTransform.position - enemy.transform.position).normalized;
        // Have to instantiate it through the rigidbody to be able to set the linear
        // velocity for some reason.
        Rigidbody2D bullet = Instantiate(bulletRB, enemy.transform.position, Quaternion.identity);
        if (!bullet.TryGetComponent<Bullet>(out var bulletScript))
        {
            Debug.LogError("Could not find bullet script");
            return;
        }
        bulletScript.DamageValue = currentAbility.Damage;
        bullet.linearVelocity = dir * bulletSpeed;
    }

    protected override WeaponAbilityLogic LoadAttack()
    {
        WeaponAbilityLogic ability = GetWeaponAbility<RangedShooting>();
        if (ability == null)
        {
            Debug.LogWarning($"{ability} not found in weapon ability list.");
        }
        return ability;
    }
}
