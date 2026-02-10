using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float shootCooldown = 0.15f;
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private float chargingBufferTime = 0.2f;
    [SerializeField] private float chargedShootTime = 1.0f;
    [SerializeField] private NewTestBullet bulletPrefab;
    [SerializeField] private float finalBulletScale = 2f;

    private float chargingBufferTimer;
    private float chargedShootTimer;
    private float shootTimer;
    private bool isChargingShot;
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
            
            if(!isChargingShot)
            {
                chargingBufferTimer += Time.deltaTime;
                if(chargingBufferTimer >= chargingBufferTime)
                {
                    isChargingShot = true;
                    CombatEvents.OnChargingShotStart?.Invoke();
                }
                return;
            }
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
        CombatEvents.OnPlayerShoot?.Invoke(false);
    }

    private void ReleaseChargedShot()
    {
        if (chargedShootTimer >= chargedShootTime)
        {
            NewTestBullet bullet = PoolManager.SpawnObject(bulletPrefab, transform.position + shootOffset, Quaternion.identity, PoolManager.PoolType.Bullets);
            bullet.ConfigureBullet(Vector2.up, bulletSpeed, false);
            bullet.ReescaleBullet(finalBulletScale);
            bullet.ChargedBullet();
            
            CombatEvents.OnChargingShotEnd?.Invoke();
        }
        
        isChargingShot = false;
        chargedShootTimer = 0f;
        chargingBufferTimer = 0f;
    }

    public float GetChargePercentage()
    {
        return Mathf.Clamp01(chargedShootTimer / chargedShootTime);
    }
}
