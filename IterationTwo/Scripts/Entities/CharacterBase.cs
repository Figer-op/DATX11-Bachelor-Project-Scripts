using System;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IDamageable
{
    [field: Min(1)]
    [field: SerializeField]
    public float MaxHealth { get; private set; } = 100f;
    [field: SerializeField]
    public float CurrentHealth { get; protected set; }

    [SerializeField]
    private GameObject weaponPrefab;
    public WeaponBase WeaponBase { get; protected set; }

    public event Action<float> OnBeingAttacked;
    public event Action<float> OnBeingHealed;

    protected virtual void Awake()
    {
        CurrentHealth = MaxHealth;
        if (weaponPrefab == null)
        {
            Debug.Log("Weapon prefab is not assigned in the inspector.");
            return;
        }
        EquipNewWeapon(weaponPrefab);
    }

    public virtual void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        OnBeingAttacked?.Invoke(amount);
        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public virtual void Heal(float amount)
    {
        float oldHealth = CurrentHealth;
        CurrentHealth = Math.Min(MaxHealth, CurrentHealth + amount);
        float healedAmount = CurrentHealth - oldHealth;

        OnBeingHealed?.Invoke(healedAmount);
    }

    // Should be called whenever a NEW weapon is assigned to player (or enemy)
    public virtual void EquipNewWeapon(GameObject newWeapon)
    {
        WeaponBase = newWeapon.GetComponent<WeaponBase>();
        WeaponBase.EquippingWeapon(gameObject.transform);
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
