using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] int damageAmount = 1;
    [SerializeField] bool instantKill = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamage(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryDamage(collision);
    }

    private void TryDamage(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        PlayerHealth health = collider.GetComponent<PlayerHealth>();

        if(health != null)
        {
            if(instantKill)
            {
                health.KillInstantly();
            }
            else
            {
                health.Die();
            }
        }
    }
}
