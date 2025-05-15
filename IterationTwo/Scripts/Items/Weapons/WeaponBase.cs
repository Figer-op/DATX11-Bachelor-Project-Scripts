using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WeaponBase : MonoBehaviour, IWeaponBase
{
    [field: SerializeField]
    public string WeaponName { get; private set; } = "NAME THIS WEAPON";
    [SerializeField, Min(0)]
    private int gizmoAbilityIndex = 0;

    [SerializeField]
    private Collider2D interactableCollider; // Used by Player class
    [SerializeField]
    private SpriteRenderer spriteRenderer; // Turned on and off depending on being carried or not

    // Weapon's available abilities are assigned manually in the inspector.
    // IMPORTANT: each weapon can only have one instance of an asset.
    //      This restriction is for duplicating abilities for the same weapon (it's about the ability list of each weapon)
    //      Can do: have CoolSword, BastardSword, WeirdSword all use the same SO asset SwordSlash
    //      Can't do: CoolSword having more than one SO asset SwordSlash, need to create a different SO class for the different slash
    [SerializeField]
    private List<WeaponAbilityLogic> availableAbilities;
    public IReadOnlyList<WeaponAbilityLogic> WeaponAvailableAbilities => availableAbilities; // public gettable of the list
    [field: SerializeField]
    public AssetReference WeaponBaseAddress { get; private set; } // Addressable reference to the weapon prefab



    public WeaponAbilityLogic GetAbility<T>() where T : WeaponAbilityLogic
    {
        T ability = availableAbilities.OfType<T>().FirstOrDefault();
        if (ability == null)
        {
            Debug.LogWarning($"{typeof(T).Name} not found in weapon ability list.");
        }
        return ability;
    }

    private void Awake()
    {
        interactableCollider.enabled = true;
    }

    // Handles weapon's prospective of the equipping process.
    public void EquippingWeapon(Transform holderTransform)
    {
        transform.SetParent(holderTransform);
        // Hide weapon when being held and disable interactions
        spriteRenderer.enabled = false;
        interactableCollider.enabled = false;

        Debug.Log($"{holderTransform.name} equipped {gameObject.name}");
    }

    // Handles weapon's prospective of the dropping process (discarding is handled by the holder's class)
    public void DroppingWeapon(Vector2 position)
    {
        transform.SetParent(null);
        // Position weapon where it was droped and enable it's renderer and interaction collider
        transform.position = position;
        transform.rotation = Quaternion.identity;
        spriteRenderer.enabled = true;
        interactableCollider.enabled = true;

        Debug.Log($"{gameObject.name} dropped at {position}");
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        WeaponAbilityLogic abilityGizmo = FindAbilityGizmo(gizmoAbilityIndex);
        Transform holderTransform = transform.parent;
        if (abilityGizmo != null && holderTransform != null)
        {
            abilityGizmo.DrawAttackZone(holderTransform);
        }
    }

    private WeaponAbilityLogic FindAbilityGizmo(int gizmoAbilityIndex)
    {
        try
        {
            return WeaponAvailableAbilities.ElementAt(gizmoAbilityIndex);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.LogError($"Choose a valid index, valid indices are 0 to {WeaponAvailableAbilities.Count - 1}");
            return null;
        }
    }
#endif
}
