using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackBaseSO : EnemyBehaviourBaseSO
{
    private bool isAttacking = false;

    // Use this list to cycle through enemy weapon's abilities without addressing them individually.
    protected IEnumerable<WeaponAbilityLogic> WeaponAbilities => enemy.WeaponBase == null ? null : enemy.WeaponBase.WeaponAvailableAbilities;

    public event Action OnEnemyAttackStart;

    private WeaponAbilityLogic currentSelectedAbility;

    public override void Initialize(EnemyBase enemy)
    {
        base.Initialize(enemy);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (playerTransform == null)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyIdleBaseInstance);
        }
        else if (!enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyChaseBaseInstance);
        } 
        else if (!isAttacking)
        {
            enemy.StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        if (isAttacking)
        {
            yield break;
        }
        isAttacking = true;
        currentSelectedAbility = LoadAttack();
        if (currentSelectedAbility == null)
        {
            Debug.LogError("Could not load enemy attack!");
            yield return null;
        }
        OnEnemyAttackStart?.Invoke();
        yield return new WaitForSeconds(currentSelectedAbility.AttackDelay);
        Attack(currentSelectedAbility);
        yield return new WaitForSeconds(currentSelectedAbility.AttackCooldown);
        isAttacking = false;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    protected abstract WeaponAbilityLogic LoadAttack();

    protected abstract void Attack(WeaponAbilityLogic currentAbility);

    // Retrieve a specific weapon ability by type
    protected WeaponAbilityLogic GetWeaponAbility<T>() where T : WeaponAbilityLogic
    {
        if (enemy.WeaponBase == null)
        {
            Debug.LogWarning("Could not retrieve weapon as WeaponBase is null!");
            return null;
        }

        return enemy.WeaponBase.GetAbility<T>();
    }
}
