using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float respawnDelay = 1f;
    [SerializeField] int maxHealth = 3;
    
    private int currentHealth;
    private Vector3 respawnPoint;
    private bool isRespawning = false;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D col;
    private PlayerController controller;

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        respawnPoint = transform.position;
    }

    public void Die()
    {
        if (isRespawning) return;

        currentHealth--;

        if(currentHealth > 0)
        {
            StartCoroutine(RespawnAfterDelay());
        }
        else
        {
            Debug.Log("Game Over - return to save point (not implemented)");
        }
    }

    public void KillInstantly()
    {
        currentHealth = 1;
        Die();
    }

    private IEnumerator RespawnAfterDelay()
    {
        isRespawning = true;
        spriteRenderer.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        col.enabled = false;
        controller.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        transform.position = respawnPoint;
        spriteRenderer.enabled = true;
        rb.simulated = true;
        col.enabled = true;
        controller.enabled = true;

        isRespawning = false;
    }

    public void SetRespawnPoint(Vector3 newPoint)
    {
        respawnPoint = newPoint;
    }
}
