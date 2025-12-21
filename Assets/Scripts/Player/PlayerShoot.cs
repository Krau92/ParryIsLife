using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float shootCooldown = 0.5f;
    [SerializeField] private float bulletSpeed = 1f;

    private float shootTimer;
    [SerializeField] Vector3 shootOffset = new Vector3(0f, 0f, 0f);

    void OnEnable()
    {
        InputManager.onShootInput += Shoot;
    }

    void OnDisable()
    {
        InputManager.onShootInput -= Shoot;
    }

    void Update()
    {
        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
    }
    

    private void Shoot()
    {
        if (shootTimer > 0f) return;
        shootTimer = shootCooldown;
        TestBullet bullet = PlayerBulletPool.Instance.Get();
        bullet.SetOwnerLayer(false); //Always doing bc maybe we want an enemy that reflects player bullets

        bullet.SetOrigin(transform.position + shootOffset);
        bullet.SetDirection(Vector2.up);
        bullet.SetSpeed(bulletSpeed);
        bullet.SetCircularSpeed(0f);
    }
}
