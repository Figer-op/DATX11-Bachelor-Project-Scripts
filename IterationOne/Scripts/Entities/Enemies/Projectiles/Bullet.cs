using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float DamageValue { private get; set; }

    [SerializeField]
    private float bulletDuration = 2;

    private void Start()
    {
        Destroy(gameObject, bulletDuration);
        if (!TryGetComponent<Rigidbody2D>(out var rb))
        {
            Debug.LogError("No rigid body found in bullet game object!");
            return;
        }
        float angle = Mathf.Atan2(rb.linearVelocityY, rb.linearVelocityX) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<PlayerBase>(out var playerBase))
        {
            playerBase.TakeDamage(DamageValue);
        }
        Destroy(gameObject);
    }
}
