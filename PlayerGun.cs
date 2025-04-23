using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePointStanding;
    [SerializeField] Transform firePointCrouching;
    [SerializeField] float fireRate = 0.25f;
    [SerializeField] KeyCode fireKey = KeyCode.Z;

    [Header("SpreadShot")]
    [SerializeField] int spreadCount = 3;
    [SerializeField] float spreadAngle = 15f;
    [SerializeField] bool unlockSpreadShot = false;

    [Header("ChargeShot")]
    [SerializeField] GameObject chargeShotPrefab;
    [SerializeField] float chargeTime = 1.5f;
    [SerializeField] bool unlockChargeShot = false;

    private float lastFireTime;
    private PlayerController playerController;
    private float chargeTimer = 0f;
    private bool isCharging = false;
    private bool canFire = true;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        Transform firePoint = playerController.isCrouching ? firePointCrouching : firePointStanding;
        firePoint.right = playerController.aimDirection;

        if (Input.GetKeyDown(fireKey) && unlockChargeShot)
        {
            isCharging = true;
            chargeTimer = 0f;
            canFire = false;
        }

        if(unlockChargeShot && isCharging)
        {
            chargeTimer += Time.deltaTime;

            if(Input.GetKeyUp(fireKey))
            {
                isCharging = false;
                canFire = true;

                if(chargeTimer >= chargeTime)
                {
                    FireChargeShot();
                }
                else if(Time.time >= lastFireTime + fireRate)
                {
                    Fire();
                }
            }
        }
        else if(!unlockChargeShot && Input.GetKeyDown(fireKey) && Time.time >= lastFireTime + fireRate)
        {
            Fire();
        }
    }

    private void Fire()
    {
        lastFireTime = Time.time;
        Vector2 direction = playerController.aimDirection;
        Transform firePoint = playerController.isCrouching ? firePointCrouching : firePointStanding;
        
        if(unlockSpreadShot)
        {
            float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            int midIndex = spreadCount / 2;

            for(int i = 0; i < spreadCount; i++)
            {
                float offset = (i - midIndex) * spreadAngle;
                float finalAngle = baseAngle + offset;
                Vector2 shotDirection = new Vector2(Mathf.Cos(finalAngle * Mathf.Deg2Rad), Mathf.Sin(finalAngle * Mathf.Deg2Rad));
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().Initialize(shotDirection.normalized);
            }
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Initialize(direction.normalized);
        }
    }

    private void FireChargeShot()
    {
        lastFireTime = Time.time;
        Transform firePoint = playerController.isCrouching ? firePointCrouching : firePointStanding;
        GameObject bullet = Instantiate(chargeShotPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Initialize(playerController.aimDirection.normalized);
    }
}
