using UnityEngine;

public class CrumblePlatform : MonoBehaviour
{
    [SerializeField] float crumbleDelay = 0.5f;
    [SerializeField] float respawnDelay = 3f;
    [SerializeField] bool respawn = true;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private CapsuleCollider2D capsuleCollider;
    private Animator anim;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            anim.SetTrigger("crumble");
            Invoke(nameof(Crumble), crumbleDelay);
        }
    }

    private void Crumble()
    {
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        capsuleCollider.enabled = false;

        if(respawn)
        {
            Invoke(nameof(Respawn), respawnDelay);
        }
    }

    private void Respawn()
    {
        anim.SetTrigger("respawn");
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        capsuleCollider.enabled = true;
    }
}
