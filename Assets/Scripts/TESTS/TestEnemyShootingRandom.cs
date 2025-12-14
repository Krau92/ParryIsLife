using UnityEngine;
//!OBSOLETE
public class TestEnemyShootingRandom : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Vector2 shootOffset;
    [SerializeField] private float shootInterval = 0.05f;

    private float shootTimer;
    [SerializeField] private Transform playerTransform;

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }

    private void ShootAtPlayer()
    {
        Vector3 shootPosition = transform.position + (Vector3)shootOffset;
        Vector3 directionShoot = new Vector3(Random.Range(0, 1f), Random.Range(-1f, 0f), 0f).normalized;

        GameObject bullet = Instantiate(bulletPrefab, shootPosition, Quaternion.LookRotation(Vector3.forward, directionShoot));
        bullet.GetComponentInChildren<TestBullet>().SetDirection(directionShoot);
        // Assuming the bullet has a script to handle its movement
    }
}
