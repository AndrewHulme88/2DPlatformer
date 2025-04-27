using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType { Health, ChargeShot, SpreadShot, MaxHealth }

    [SerializeField] PickupType pickupType;
    [SerializeField] int healAmount = 1;
    [SerializeField] GameObject pickupParticles;
    [SerializeField] bool doesDisappear = false;
    [SerializeField] float lifetimeAfterDrop = 10f;
    [SerializeField] int healthIncreaseAmount = 1;
     
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if(doesDisappear)
        {
            Destroy(gameObject, lifetimeAfterDrop);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        PlayerGun gun = collision.GetComponent<PlayerGun>();

        if(pickupParticles != null)
        {
            Instantiate(pickupParticles, transform.position, Quaternion.identity);
        }

        switch (pickupType)
        {
            case PickupType.Health:
                player?.Heal(healAmount);
                break;

            case PickupType.ChargeShot:
                if(gun != null)
                {
                    gun.UnlockChargeShot();
                }
                break;

            case PickupType.SpreadShot:
                if(gun != null)
                {
                    gun.UnlockSpreadShot();
                }
                break;

            case PickupType.MaxHealth:
                if(player != null)
                {
                    player.IncreaseMaxHealth(healthIncreaseAmount);
                    UI ui = FindFirstObjectByType<UI>();
                    ui.UpdateHealthUI();
                }
                break;
        }

        anim.SetTrigger("destroy");
    }

    public void DestroyPickup()
    {
        Destroy(gameObject);
    }
}
