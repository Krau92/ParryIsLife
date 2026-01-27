using UnityEngine;


public class NewTestBullet : MonoBehaviour
{
    private Vector2 origin;
    private float radius;
    private float angle;

    [SerializeField] private int playerBulletLayer = 0; // set in Inspector to the PlayerBullet layer index
    [SerializeField] private int enemyBulletLayer = 0;  // set in Inspector to the EnemyBullet layer index

    private Vector2 direction;
    private Vector2 defaultScale;

    private float speed = 5f;
    private float circularSpeed = 0f;

    private float deathTimer = 0f;
    const float defaultDeathDuration = 2.0f;

    

    private bool reflected;
    public bool IsReflected() { return reflected; }

    private bool charged;
    public bool IsCharged() { return charged; }

    void Awake()
    {
        defaultScale = transform.localScale;
    }

    void OnDisable()
    {
        CancelInvoke("DeactivateBullet");
    }

    void FixedUpdate()
    {
        // Actualizar física y posición en sync con el motor de física
        radius += speed * Time.fixedDeltaTime;
        if (circularSpeed != 0f)
        {
            angle += circularSpeed * Time.fixedDeltaTime;
            direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }

    void LateUpdate()
    {
        // Aplicar posición después que todo está calculado
        transform.position = origin + direction * radius;
    }

    public void ConfigureBullet(Vector2 dir, float spd, bool enemyBullet, Vector2 newOrigin = default)
    {
        reflected = false;
        deathTimer = defaultDeathDuration;
        if (newOrigin == default)
            SetOrigin();
        else
            SetOrigin(newOrigin);
        
        SetDirection(dir);
        SetSpeed(spd);
        SetOwnerLayer(enemyBullet);
        SetCircularSpeed(0f);

        CancelInvoke("DeactivateBullet");
        Invoke("DeactivateBullet", deathTimer);
    }

    public void ConfigureBullet(Vector2 dir, float spd, bool enemyBullet, float deathTime)
    {
        reflected = false;
        deathTimer = deathTime;
        SetOrigin();
        SetDirection(dir);
        SetSpeed(spd);
        SetOwnerLayer(enemyBullet);
        SetCircularSpeed(0f);

        CancelInvoke("DeactivateBullet");
        Invoke("DeactivateBullet", deathTimer);

    }

    private void SetOrigin()
    {
        origin = transform.position;
        radius = 0f;
    }

    private void SetOrigin(Vector2 newOrigin)
    {
        transform.position = newOrigin;
        origin = newOrigin;
        radius = 0f;
    }

    private void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void SetSpeed(float newSpeed)
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

    private void SetOwnerLayer(bool isEnemy)
    {
        gameObject.layer = isEnemy ? enemyBulletLayer : playerBulletLayer;
    }

    public void DeactivateBullet()
    {
        ResetBullet();
        PoolManager.ReturnObjectToPool(this.gameObject, PoolManager.PoolType.Bullets);
    }

    public void ReflectBullet(Vector2 newOrigin)
    {
        reflected = true;
        Vector2 newDirection = -direction;

        if(gameObject.layer == enemyBulletLayer)
        {
            SetOwnerLayer(false); // Change to player bullet
        }
        else if(gameObject.layer == playerBulletLayer)
        {
            SetOwnerLayer(true); // Change to enemy bullet
        }

        SetDirection(newDirection);
        SetOrigin(newOrigin);
        SetCircularSpeed(0f);

        //Reset death timer
        CancelInvoke("DeactivateBullet");
        Invoke("DeactivateBullet", deathTimer);

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.yellow; // Change color to indicate reflection
    }

    public void ReescaleBullet(float scaleFactor)
    {
        transform.localScale = defaultScale * scaleFactor;
    }

    public void ResetBullet()
    {
        //Reset scale, color, reflected and charged state
        transform.localScale = defaultScale;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
        reflected = false;
        charged = false;
    }

    public void ChargedBullet()
    {
        charged = true;
    }


}
