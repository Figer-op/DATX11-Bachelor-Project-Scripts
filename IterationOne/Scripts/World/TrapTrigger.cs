using System.Collections;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField]
    private Sprite passiveSprite;
    [SerializeField]
    private Sprite activeSprite;
    [SerializeField]
    private float damage = 25f;

    public void Activate()
    {
        GetComponent<SpriteRenderer>().sprite = activeSprite;
        StartCoroutine(DeactivateAfterDelay(0.5f));
    }

    public void Deactivate()
    {
        GetComponent<SpriteRenderer>().sprite = passiveSprite;
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerBase>(out var character))
        {
            Activate();
            character.TakeDamage(damage);
        }
    }
}