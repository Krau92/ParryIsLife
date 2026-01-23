using System.Collections.Generic;
using UnityEngine;

public class TestNewPoolShooting : MonoBehaviour
{
    [SerializeField] private NewTestBullet bulletPrefab;
    [SerializeField] private float shootInterval = 0.2f;
    [SerializeField] private float burstsInterval = 1f;
    [SerializeField] private int bulletsPerShot = 5;
    [SerializeField] private float spreadAngle = 15f;
    [SerializeField] private int shootPerBurst = 6;
    [SerializeField] private float bulletSpeed = 5f;


    [SerializeField] private List<Transform> testingParents;
    Transform playerTransform;


    private float shootTimer = 0f;
    private float burstTimer = 0f;
    private int burstsShot = 0;
    private int shootCounter = 0;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        burstTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;

        if (burstTimer <= burstsInterval)
        {
            if (burstsShot < shootPerBurst && shootTimer >= shootInterval)
            {
                ShootBullets();
                burstsShot++;
                shootTimer = 0f;
            }
        }
        else
        {
            shootCounter++;
            shootCounter %= testingParents.Count;
            shootTimer = 0f; // Reset shoot timer between bursts
            burstTimer = 0f;
            burstsShot = 0;
        }
    }

    private void ShootBullets()
    {
        Vector2 directionToPlayer = (playerTransform.position - testingParents[shootCounter].position).normalized;
        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x);
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Transform parent = testingParents[shootCounter];
            NewTestBullet obj = PoolManager.SpawnObject(bulletPrefab, parent, Quaternion.identity, PoolManager.PoolType.Bullets);
            float angleOffset = baseAngle - spreadAngle / 2 + (spreadAngle / (bulletsPerShot - 1)) * i;
            Vector2 direction = new Vector2(Mathf.Cos(angleOffset), Mathf.Sin(angleOffset));  
            obj.ConfigureBullet(direction, bulletSpeed, true, (Vector2)parent.position);
        }
    }
}
