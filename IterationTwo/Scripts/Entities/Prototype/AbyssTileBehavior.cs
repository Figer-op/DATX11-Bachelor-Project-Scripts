using UnityEngine;

public class AbyssTileBehavior : MonoBehaviour
{
    // This script should be attached to a child object of the abyss tile with a collider that is a trigger!
    private void OnTriggerEnter2D(Collider2D collison)
    {
        if (!collison.isTrigger)
        {
            if (collison.TryGetComponent<PlayerBase>(out var player))
            {
                player.TakeDamage(player.MaxHealth);
            }
        }
    }
}
