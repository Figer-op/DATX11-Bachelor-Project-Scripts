using System.Collections.Generic;
using UnityEngine;
interface IWeaponBase
{
    void EquippingWeapon(Transform holderTransform);
    void DroppingWeapon(Vector2 position);
    IReadOnlyList<WeaponAbilityLogic> WeaponAvailableAbilities { get; }
}
