using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Cooldown", menuName = "Abilities/Properties/Cooldown")]
public class CooldownSO : ScriptableObject
{
    [SerializeField]
    private float cooldownTime = 2f;
    private float cooldownTimer;
    public bool IsOnCooldown { get; private set; }

    public event Action<string, float> OnCooldownChanged;

    public void OnUpdateLogic()
    {
        if (IsOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            OnCooldownChanged?.Invoke(name, cooldownTimer);

            if (cooldownTimer <= 0)
            {
                IsOnCooldown = false;
                cooldownTimer = cooldownTime;
            }
        }
    }

    public void TriggerCooldown()
    {
        if (!IsOnCooldown)
        {
            IsOnCooldown = true;
        }
    }
}