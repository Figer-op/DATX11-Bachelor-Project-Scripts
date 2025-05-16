using UnityEngine;
using System.Collections;

public class FireballShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject fireballPrefab; // Prefab for the fireball
    [SerializeField]
    private float shootDelay = 0f; // Delay before shooting the fireball
    [SerializeField]
    private float cooldownTime = 2f; // Time between fireball shots
    private bool canShoot = true; // Flag to check if the shooter can shoot

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canShoot)
        {
            // Check if the object that entered the trigger is a player
            if (collision.CompareTag("Player"))
            {
                StartCoroutine(ShootFireball());
            }
        }
    }

    private IEnumerator ShootFireball()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        // Instantiate the fireball at the position of this object
        GameObject fireball = Instantiate(fireballPrefab, transform.position, transform.rotation);
        // Start the cooldown coroutine
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        // Wait for the cooldown time
        yield return new WaitForSeconds(cooldownTime);
        // Allow shooting again
        canShoot = true;
    }

    private void OnDrawGizmos()
    {
        // Draw a red sphere at the position of this object to visualize the origin
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}