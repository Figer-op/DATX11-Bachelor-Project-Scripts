using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CapsuleCollider2D))]
public class StrikingDistanceCheck : TargetCheck
{
    [SerializeField]
    private WeaponAbilityLogic ability; // TODO: do this the non-lazy way
    private void Start()
    {
        if (ability == null) 
        {
            Debug.LogWarning("Enemy weapon doesn't have a reference to an ability to create an attack zone for it");
            return;  
        }
        ColliderSetup();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            enemy.IsWithinStrikingDistance = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            enemy.IsWithinStrikingDistance = false;
        }
    }

    private void ColliderSetup()
    {
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        // feel free to remove this and isTrigger if it is triggering u.
        capsule.enabled = true; // turning it off for the prefabs in editor mode by default since it will be mismatched before Starting
        capsule.direction = ability.Width > ability.Length ? CapsuleDirection2D.Vertical : CapsuleDirection2D.Horizontal;
        capsule.size = new Vector2(ability.Length, ability.Width);
        // adjusted object position instead of the collider offset because it is much more precise for some reason
        transform.position = new Vector2(transform.position.x + ability.ForwardOffset, transform.position.y);
        transform.rotation = Quaternion.Euler(0, 0, ability.AngleOffset);
        capsule.isTrigger = true; // Just in case it was added by RequireComponent
    }
}
