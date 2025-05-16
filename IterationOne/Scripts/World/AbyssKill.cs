using UnityEngine;

public class AbyssKill : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 playerCenter = collision.gameObject.transform.position;
            
            if (collision.otherCollider.OverlapPoint(playerCenter))
            {
                PlayerBase playerBase = collision.gameObject.GetComponent<PlayerBase>();
                playerBase.TakeDamage(playerBase.MaxHealth);
            }
        }
    }
}
