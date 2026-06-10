using UnityEngine;

public class MinionBarrier : MonoBehaviour
{
    [HideInInspector] public NecromancerBoss necromancerBoss;
    float maxHealth = 15f;
    public float movingVelocity = 4f;
    Transform positionPoint;
    [HideInInspector] public Transform playerTransform;
    private float currentHealth;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(positionPoint != null && transform.position != positionPoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionPoint.position, movingVelocity * Time.deltaTime);
        }
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
