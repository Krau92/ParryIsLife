using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float shootCooldown = 0.15f;
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private float chargedShootTime = 1.0f;
    [SerializeField] private NewTestBullet bulletPrefab;
    [SerializeField] private float finalBulletScale = 2f;

    private float chargedShootTimer;
    private float shootTimer;
    [SerializeField] Vector3 shootOffset = new Vector3(0f, 0f, 0f);

    void OnEnable()
    {
        InputManager.onShootInput += Shoot;
        InputManager.onStopShootInput += ReleaseChargedShot;
    }

    void OnDisable()
    {
        InputManager.onShootInput -= Shoot;
        InputManager.onStopShootInput -= ReleaseChargedShot;
    }

    void Update()
    {
        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
        if (InputManager.Instance.IsShootHold())
        {
            if(chargedShootTimer < chargedShootTime)
            {
                chargedShootTimer += Time.deltaTime;
            } 

        }
    }
    

    private void Shoot()
    {
        if (shootTimer > 0f) return;
        shootTimer = shootCooldown;
        NewTestBullet bullet = PoolManager.SpawnObject(bulletPrefab, transform.position + shootOffset, Quaternion.identity, PoolManager.PoolType.Bullets);
        bullet.ConfigureBullet(Vector2.up, bulletSpeed, false);
    }

    private void ReleaseChargedShot()
    {
        if (chargedShootTimer >= chargedShootTime)
        {
            NewTestBullet bullet = PoolManager.SpawnObject(bulletPrefab, transform.position + shootOffset, Quaternion.identity, PoolManager.PoolType.Bullets);
            bullet.ConfigureBullet(Vector2.up, bulletSpeed, false);
            bullet.ReescaleBullet(finalBulletScale);
        }
        chargedShootTimer = 0f;
    }
}
