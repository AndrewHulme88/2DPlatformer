using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;
    [SerializeField] float invicibilityTimer = 1f;
    
    public bool isInvincible = false;

    private int currentHealth;

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible || currentHealth <= 0) return;

        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFlash());
        }
    }

    private void Die()
    {
        Debug.Log("Player died");
    }

    private IEnumerator InvincibilityFlash()
    {
        isInvincible = true;

        float timeElapsed = 0f;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        while(timeElapsed < invicibilityTimer)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            timeElapsed += 0.1f;
        }

        sr.enabled = true;
        isInvincible = false;
    }
}
