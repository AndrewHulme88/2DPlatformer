using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType { Player, Enemy }

    [SerializeField] float speed = 10f;
    [SerializeField] float lifeTime = 2f;
    [SerializeField] GameObject hitParticles;
    [SerializeField] BulletType bulletType;

    public int damageAmount = 1;

    private Vector2 direction;
    private Animator anim;
    private bool isHit = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (isHit) return;

        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(bulletType == BulletType.Player)
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();


            if(enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        }

        if(bulletType == BulletType.Enemy)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if(playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }

        Instantiate(hitParticles, transform.position, Quaternion.identity);
        isHit = true;

        Destroy(gameObject);
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
