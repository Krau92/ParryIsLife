using UnityEngine;

public class MinionBarrier : MonoBehaviour
{
    [HideInInspector] public NecromancerBoss necromancerBoss;
    [SerializeField] float timeBetweenShoots;
    [SerializeField] ShootingPatternSO shootingPattern;
    [SerializeField] NewTestBullet bulletPrefab;
    float maxHealth = 15f;
    public float movingVelocity = 4f;
    float shootTimer;
    Transform positionPoint;
    [HideInInspector] public Transform playerTransform;
    private float currentHealth;

    void Update()
    {
        if(positionPoint != null && transform.position != positionPoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionPoint.position, movingVelocity * Time.deltaTime);
        }
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0f)
        {
            shootTimer = timeBetweenShoots;
            StartCoroutine(shootingPattern.Shoot(transform.position, playerTransform.position, bulletPrefab));
        }

    }

    public void SetTimeBetweenShoots(float time)
    {
        timeBetweenShoots = time;
        shootTimer = time;
    }

        public void SetTimerDelay(float time)
    {
        shootTimer += time;
    }

    public void SetHealth(float health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
    }

    public void SetNewPosition(Transform newPosition)
    {
        positionPoint = newPosition;
    }

    void TakeDamage()
    {
        currentHealth -= 1f;
        if (currentHealth <= 0f)
        {
            necromancerBoss.OnMinionBarrierDestroyed(this);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            NewTestBullet bullet = collision.GetComponent<NewTestBullet>();
            if(bullet != null && bullet.IsReflected())
            {
                TakeDamage();
            }
            bullet.DeactivateBullet();
        }
    }
    

}
