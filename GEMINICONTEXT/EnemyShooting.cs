using NUnit.Framework.Internal;
using UnityEngine;


public class EnemyShooting : MonoBehaviour
{
    //TODO: Implementar la lista de patterns de disparo
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval = 3f;

    private float shootTimer;
    private int shootCounter;


    private void Shoot(float speed, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject bullet = PoolManager.Instance.GetGameObjectFromPool(bulletPrefab);
            Vector2 directionShoot = new Vector2(i, -(amount/2)).normalized;
            bullet.transform.position = transform.position;

            //TODO: Create a parent class Bullet to avoid using TestBullet here
            TestBullet bulletScript = bullet.GetComponent<TestBullet>();
            bulletScript.SetDirection(directionShoot);
            bulletScript.SetSpeed(speed);

            bullet.SetActive(true);
        }
    }

    void Update()
    {
        //For testing purposes, shoot every interval
        shootTimer += Time.deltaTime;
        bool canShoot = shootTimer >= shootInterval;
        if (canShoot && shootCounter == 3)
        {
            Shoot(5.0f, 10);
            shootTimer = 0f;
            shootCounter = 0;
        }

        if (canShoot && shootCounter != 3)
        {
            Shoot(3.0f, 15);
            shootTimer = 0f;
            shootCounter++;
        }
    }
}
