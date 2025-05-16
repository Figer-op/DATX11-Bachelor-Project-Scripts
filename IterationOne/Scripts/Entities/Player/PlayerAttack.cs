using System.Linq;
using UnityEngine;
using System.Collections;
using System;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private PlayerBase player;

    private bool isAttacking = false;
    private bool isStunned = false; // set true currently when player is damaged

    [SerializeField]
    private float stunDuration = 3f; // TODO: needs to be adjusted according to player being hit animation

    public event Action OnPlayerAttackStart;
    public event Action OnPlayerAttackEnd;

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.OnAttackPressed += HandleAttack;
        }
        else Debug.LogError("PlayerInput not assigned");

        if (player != null)
        {
            player.OnBeingAttacked += HandleBeingAttacked;
        }
        else Debug.LogError("Player class not assigned");
    }
    private void OnDisable()
    {
        player.OnBeingAttacked -= HandleBeingAttacked;
    }

    private void HandleAttack(int abilityIndex)
    {
        if (player.PlayerWeaponAbilities == null)
        {
            Debug.LogError("Player's current weapon doesn't have any abilities");
            return;           
        }
        if (!isAttacking && !isStunned)
        {
            isAttacking = true;
            StartCoroutine(AttackRoutine(abilityIndex));
        }
        else if (isStunned) Debug.Log("can't attack, currently stunned"); // temp. Remove after this PR
    }

    private void HandleBeingAttacked(float _) // float parameter due to lazy one event for both the UI and this class
    {
        StopCoroutine(nameof(AttackRoutine));
        isStunned = true;
        StartCoroutine(StunnedRoutine());
    }

    private IEnumerator AttackRoutine(int abilityIndex)
    {
        WeaponAbilityLogic ability = LoadAttack(abilityIndex);
        if (ability == null)
        {
            Debug.LogError("Could not load player attack!");
            isAttacking = false;
            yield break;
        }
        OnPlayerAttackStart?.Invoke();
        yield return new WaitForSeconds(ability.AttackDelay);

        ability.UseAbility(transform, 1 << gameObject.layer);

        yield return new WaitForSeconds(ability.AttackCooldown);

        OnPlayerAttackEnd?.Invoke();
        isAttacking = false;
    }

    private WeaponAbilityLogic LoadAttack(int abilityIndex)
    {
        if (abilityIndex > player.PlayerWeaponAbilities.Count() - 1)
        {
            Debug.LogError("Attempt to use a non-existing weapon ability by player");
            return null;
        }
        // Usage of ability depending on mouse click (zero-indexed).
        return player.PlayerWeaponAbilities.ElementAt(abilityIndex);
    }

    private IEnumerator StunnedRoutine()
    {   
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }
}
