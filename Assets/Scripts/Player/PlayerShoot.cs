using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float shootCooldown = 0.15f;
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private float chargingBufferTime = 0.2f;
    [SerializeField] private float chargedShootTime = 1.0f;
    [SerializeField] private NewTestBullet bulletPrefab;
    [SerializeField] private float finalBulletScale = 2f;
    [SerializeField] private SoundEffectSO shootSoundEffect, chargedShootSoundEffect;

    private float chargingBufferTimer;
    private float chargedShootTimer;
    private float shootTimer;
    private bool isChargingShot;
    [SerializeField] Vector3 shootOffset = new Vector3(0f, 0f, 0f);

    void OnEnable()
    {
        InputManager.onShootInput += Shoot;
        InputManager.onStopShootInput += ReleaseChargedShot;
        CombatEvents.OnCombatEnded += ReleaseChargedShot;
    }

    void OnDisable()
    {
        InputManager.onShootInput -= Shoot;
        InputManager.onStopShootInput -= ReleaseChargedShot;
        CombatEvents.OnCombatEnded -= ReleaseChargedShot;
    }

    void Update()
    {
        if(GameManager.Instance.currentGameState != GameState.InCombat)
            return;


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
        //Check if we can shoot and if we are in combat
        if (shootTimer > 0f || GameManager.Instance.currentGameState != GameState.InCombat) 
            return;


        shootTimer = shootCooldown;
        NewTestBullet bullet = PoolManager.SpawnObject(bulletPrefab, transform.position + shootOffset, Quaternion.identity, PoolManager.PoolType.Bullets);
        bullet.ConfigureBullet(Vector2.up, bulletSpeed, false);
        if(shootSoundEffect != null)
        {
            shootSoundEffect.PlayEffect();
        }
    }

    private void ReleaseChargedShot()
    {
        if (chargedShootTimer >= chargedShootTime)
        {
            NewTestBullet bullet = PoolManager.SpawnObject(bulletPrefab, transform.position + shootOffset, Quaternion.identity, PoolManager.PoolType.Bullets);
            bullet.ConfigureBullet(Vector2.up, bulletSpeed, false);
            bullet.ReescaleBullet(finalBulletScale);
            bullet.ChargedBullet();

            if(chargedShootSoundEffect != null)
            {
                chargedShootSoundEffect.PlayEffect();
            }
        }

        CombatEvents.OnChargingShotEnd?.Invoke();
        
        isChargingShot = false;
        chargedShootTimer = 0f;
        chargingBufferTimer = 0f;
    }

    public float GetChargePercentage()
    {
        return Mathf.Clamp01(chargedShootTimer / chargedShootTime);
    }
}
