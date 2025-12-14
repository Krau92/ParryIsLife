using UnityEngine;
//!OBSOLETE
public class TestEnemyShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Vector2 shootOffset;
    [SerializeField] private float shootInterval = 1f;

    private float shootTimer;
    [SerializeField]private Transform playerTransform;

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
        Vector3 directionToPlayer = (playerTransform.position - shootPosition).normalized;

        GameObject bullet = Instantiate(bulletPrefab, shootPosition, Quaternion.LookRotation(Vector3.forward, directionToPlayer));
        bullet.GetComponentInChildren<TestBullet>().SetDirection(directionToPlayer);
        // Assuming the bullet has a script to handle its movement
    }
}
