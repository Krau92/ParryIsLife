using UnityEngine;

public class TestPlayerFeedback : MonoBehaviour
{
    private PlayerHealth playerHealth;
    [SerializeField] Color parryColor = Color.yellow;
    [SerializeField] Color reflectColor = Color.cyan;
    [SerializeField] Color damageColor = Color.red;
    [SerializeField] Color parryCDColor = Color.blue;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void OnEnable()
    {
        PlayerHealth.OnParryStart += FlashOnParry;
        PlayerHealth.OnParryEnd += StopFlashing;
        PlayerHealth.OnReflectingStart += FlashOnReflect;
        PlayerHealth.OnReflectingEnd += StopFlashing;
        PlayerHealth.OnDamageTaken += FlashOnDamage;
    }

    void OnDisable()
    {
        PlayerHealth.OnParryStart -= FlashOnParry;
        PlayerHealth.OnParryEnd -= StopFlashing;
        PlayerHealth.OnReflectingStart -= FlashOnReflect;
        PlayerHealth.OnReflectingEnd -= StopFlashing;
        PlayerHealth.OnDamageTaken -= FlashOnDamage;
    }

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        originalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Bullet"))
        {
            if (playerHealth != null && playerHealth.IsInmune())
            {
                return;
            }
            FlashOnDamage();
        }


    }

    public void FlashOnParry()
    {
        StartFlashing(parryColor);
    }

    public void FlashOnReflect()
    {
        StartFlashing(reflectColor);
    }

    public void FlashOnDamage()
    {
        StartFlashing(damageColor);
    }

    private void StartFlashing(Color flashColor)
    {
        spriteRenderer.color = flashColor;
    }

    private void StopFlashing()
    {
        spriteRenderer.color = originalColor;
    }
}
