using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackSword", menuName = "EnemyLogic/AttackLogic/Enemy Attack Sword")]

public class EnemyAttackSword : EnemyAttackBaseSO
{
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        enemy.Move(Vector2.zero);
    }

    protected override void Attack(WeaponAbilityLogic currentAbility)
    {
        currentAbility.UseAbility(enemy.transform, 1 << enemy.gameObject.layer);
    }

    protected override WeaponAbilityLogic LoadAttack()
    {
        WeaponAbilityLogic ability = GetWeaponAbility<SwordSlash>();
        if (ability == null)
        {
            Debug.LogWarning($"{ability} not found in weapon ability list.");
        }
        return ability;
    }
}
