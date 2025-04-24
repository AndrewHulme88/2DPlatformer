using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType { Health, ChargeShot, SpreadShot }

    [SerializeField] PickupType pickupType;
    [SerializeField] int healAmount = 1;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        PlayerGun gun = collision.GetComponent<PlayerGun>();

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
        }

        anim.SetTrigger("destroy");
    }

    public void DestroyPickup()
    {
        Destroy(gameObject);
    }
}
