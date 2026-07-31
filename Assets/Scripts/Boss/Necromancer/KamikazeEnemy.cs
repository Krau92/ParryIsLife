using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class KamikazeEnemy : MonoBehaviour
{
    [SerializeField] private float extraVerticalOffset = 0.5f;
    [SerializeField] private float stopTime = 0.5f;
    [SerializeField] private float seekSpeedReduction = 0.25f;
    [SerializeField] private int maxHealth = 4;

    private Vector2 moveToPosition;
    private float currentHealth;
    private float waitTimer;
    private float seekTimer;
    private float moveSpeed;
    private Transform target;

    private Vector2 savedPlayerPos;
    private Vector2 descendTarget;
    private SpriteRenderer spriteRenderer;

    private enum State { Waiting, Seeking, Stopped, Diving, Dead }
    private State state;

    void OnEnable()
    {
        GameEvents.OnBossDefeated += DestroyThis;
    }

    void OnDisable()
    {
        GameEvents.OnBossDefeated -= DestroyThis;
    }

    public void SetKamikaze(Vector2 spawnPosition, Vector2 moveToPosition, float waitTime, float seekTime, float moveSpeed, Transform target)
    {
        this.moveToPosition = moveToPosition;
        currentHealth = maxHealth;
        waitTimer = waitTime;
        seekTimer = seekTime;
        this.moveSpeed = moveSpeed;
        this.target = target;
        state = State.Waiting;
        transform.position = spawnPosition;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(KamikazeMovement());
    }

    private IEnumerator KamikazeMovement()
    {
        yield return WaitPhase();
        yield return SeekPhase();
        yield return StopPhase();
        yield return DivePhase();
    }

    private IEnumerator WaitPhase()
    {
        state = State.Waiting;
        while (waitTimer > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveToPosition, moveSpeed * Time.deltaTime);
            waitTimer -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator SeekPhase()
    {
        state = State.Seeking;

        float totalOffset = extraVerticalOffset;
        if (spriteRenderer != null && spriteRenderer.sprite != null)
            totalOffset += spriteRenderer.sprite.bounds.size.y * 0.5f;

        descendTarget = new Vector2(transform.position.x, transform.position.y - totalOffset);

        while (seekTimer > 0)
        {
            float newX = Mathf.MoveTowards(transform.position.x, target.position.x, moveSpeed * seekSpeedReduction * Time.deltaTime);
            float newY = Mathf.MoveTowards(transform.position.y, descendTarget.y, moveSpeed * seekSpeedReduction * Time.deltaTime);
            transform.position = new Vector2(newX, newY);
            seekTimer -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator StopPhase()
    {
        savedPlayerPos = target.position;
        state = State.Stopped;
        yield return new WaitForSeconds(stopTime);
    }

    private IEnumerator DivePhase()
    {
        state = State.Diving;
        while (state == State.Diving)
            yield return null;
    }

    private void Update()
    {
        if (state != State.Diving)
            return;

        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        float cameraBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        if (transform.position.y < cameraBottom)
            ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (state == State.Diving && other.CompareTag("Player"))
            ReturnToPool();
        if(other.CompareTag("Bullet"))
        {
            
            NewTestBullet bullet = other.gameObject.GetComponent<NewTestBullet>();
            if(bullet.IsCharged())
            {
                TakeDamage(maxHealth);
            } else
            {
                TakeDamage(1);
            }
            bullet.DeactivateBullet();
        }
    }

    private void ReturnToPool()
    {
        state = State.Dead;
        StopAllCoroutines();
        PoolManager.ReturnObjectToPool(gameObject, PoolManager.PoolType.Enemy);
    }

    private void TakeDamage(int value)
    {
        currentHealth -= value;
        if(currentHealth <= 0)
        {
            ReturnToPool();
        }
    }

    private void DestroyThis()
    {
        ReturnToPool();
    }
}
