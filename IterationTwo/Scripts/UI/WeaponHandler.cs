using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField]
    private Image weaponImage;

    [SerializeField]
    private TMP_Text weaponName;

    [SerializeField]
    private TMP_Text weaponDamage;

    [SerializeField]
    private TMP_Text weaponAttackSpeed;

    [SerializeField]
    private TMP_Text weaponRange;

    private PlayerBase playerBase; 

    private void Awake()
    {
        playerBase = FindAnyObjectByType<PlayerBase>();
    }

    private void OnEnable()
    {
        playerBase.OnNewWeaponEquipped += EquipWeapon;
    }

    private void OnDisable()
    {
        playerBase.OnNewWeaponEquipped += EquipWeapon;
    }

    private void EquipWeapon(WeaponBase newWeaponBase)
    {
        if (!newWeaponBase.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            Debug.LogError("SpriteRenderer For Equipped Weapon Not Found!");
            return;
        }

        if (newWeaponBase.WeaponAvailableAbilities.Count <= 1)
        {
            Debug.LogError("There are not at least two attacks in the provided weapon!");
            return;
        }

        WeaponAbilityLogic firstAbility = newWeaponBase.WeaponAvailableAbilities[0];
        WeaponAbilityLogic secondAbility = newWeaponBase.WeaponAvailableAbilities[1];

        string damageText = $"{firstAbility.Damage} | {secondAbility.Damage}";
        string attackSpeedText = $"{CalcAttackSpeed(firstAbility)} | {CalcAttackSpeed(secondAbility)}";
        string rangeText = $"{CalcAttackRange(firstAbility)} | {CalcAttackRange(secondAbility)}";

        weaponImage.sprite = spriteRenderer.sprite;
        weaponName.text = newWeaponBase.WeaponName;
        weaponDamage.text = damageText;
        weaponAttackSpeed.text = attackSpeedText;
        weaponRange.text = rangeText;
    }

    private static int CalcAttackSpeed(WeaponAbilityLogic ability) => Mathf.RoundToInt(100 / ability.AttackCooldown);

    private static int CalcAttackRange(WeaponAbilityLogic ability) => Mathf.RoundToInt((ability.Length/2 + ability.ForwardOffset) * 100); // Assuming Length is in meters
}
