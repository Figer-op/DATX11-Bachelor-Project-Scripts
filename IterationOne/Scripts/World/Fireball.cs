using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f; // Speed of the fireball
    [SerializeField]
    private float damage = 20f; // Damage dealt by the fireball
    [SerializeField]
    private float lifetime = 5f; // Time before the fireball is destroyed
    private Rigidbody2D rb; // Rigidbody component of the fireball

    private void Start()
    {
        if (!TryGetComponent<Rigidbody2D>(out rb))
        {
            Debug.LogError("Rigidbody2D component not found on the fireball!");
            return;
        }
        // Destroy the fireball after its lifetime
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move the fireball forward
        rb.linearVelocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<PlayerBase>(out PlayerBase playerBase))
        {
            // Deal damage to the target
            playerBase.TakeDamage(damage);
        }
        // Destroy the fireball after it hits something
        Destroy(gameObject);
    }
}
