using UnityEngine;
using UnityEngine.UIElements;

public class TestBullet : MonoBehaviour
{
    private Vector2 origin;
    private float radius;
    private float angle;

    private Vector2 direction;

    private float speed = 5f;
    private float circularSpeed = 0f;
    bool isReturningToPool = false;

    float invisibleTimer = 0f;
    float invisibleDuration = 2f;

    void OnEnable()
    {
        isReturningToPool = false; 
    }

    void Update()
    {
        radius += speed * Time.deltaTime;
        if (circularSpeed != 0f)
        {
            angle += circularSpeed * Time.deltaTime;
            direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        transform.position = origin + direction * radius;
             
    }

    public void SetOrigin(Vector2 newOrigin)
    {
        origin = newOrigin;
        radius = 0f;
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetCircularSpeed(float newCircularSpeed)
    {
        circularSpeed = newCircularSpeed * Mathf.Deg2Rad; // Convert to radians per second
        if (circularSpeed != 0f)
        {
            angle = Mathf.Atan2(direction.y, direction.x);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isReturningToPool)
        {
            // Return bullet to pool
            isReturningToPool = true;
            EnemyBulletPool.Instance.ReturnToPool(this);
        }
    }

    void OnBecameInvisible()
    {
        if(isReturningToPool) return;
        invisibleTimer += Time.deltaTime;
        if (invisibleTimer >= invisibleDuration)
        {
            isReturningToPool = true;
            EnemyBulletPool.Instance.ReturnToPool(this);
        }
    }

    void OnBecameVisible()
    {
        invisibleTimer = 0f;
    }

}
