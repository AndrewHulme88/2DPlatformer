using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    public enum ShootDirection { Up, Down, Left, Right }

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] ShootDirection shootDirection = ShootDirection.Right;
    [SerializeField] float fireRate = 2f;
    [SerializeField] float bulletSpeed = 5f;

    private float fireTimer;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;

        if(fireTimer >= fireRate)
        {
            anim.SetTrigger("shoot");
            fireTimer = 0f;
        }
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Vector2 direction = Vector2.right;

        switch(shootDirection)
        {
            case ShootDirection.Up:
                direction = Vector2.up;
                break;
            case ShootDirection.Down:
                direction = Vector2.down;
                break;
            case ShootDirection.Left:
                direction = Vector2.left;
                break;
            case ShootDirection.Right:
                direction = Vector2.right;
                break;
        }

        bullet.GetComponent<Bullet>().Initialize(direction.normalized);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
