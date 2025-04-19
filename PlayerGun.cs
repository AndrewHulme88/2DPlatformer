using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 0.25f;
    [SerializeField] KeyCode fireKey = KeyCode.Z;

    private float lastFireTime;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        firePoint.right = playerController.aimDirection;

        if (Input.GetKey(fireKey) && Time.time >= lastFireTime + fireRate)
        {
            Fire();
        }
    }

    private void Fire()
    {
        lastFireTime = Time.time;
        Vector2 direction = playerController.aimDirection;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Initialize(direction);
    }
}
