using UnityEngine;

public class TestPlayerFeedback : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] Color parryColor = Color.yellow;
    [SerializeField] Color damageColor = Color.red;
    [SerializeField] Color parryCDColor = Color.blue;

    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private float parryCD = 1f; //ALERT: coordinate with PlayerCombat parryCooldown

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private bool isFlashing = false;
    private float flashTimer = 0f;
    private float parryCDTimer = 0f;

    private void OnEnable()
    {
        InputManager.onParryInput += ParryCDSet;
    }

    private void OnDisable()
    {
        InputManager.onParryInput -= ParryCDSet;
    }

    private void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (isFlashing)
        {
            flashTimer += Time.deltaTime;
            parryCDTimer += Time.deltaTime;
            if (flashTimer >= flashDuration && parryCDTimer < parryCD)
            {
                spriteRenderer.color = parryCDColor;
            }
            else if (flashTimer >= flashDuration && parryCDTimer >= parryCD)
            {
                StopFlashing();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Bullet"))
        {
            if (playerCombat != null && playerCombat.IsParrying)
            {
                FlashOnParry();
                //Player is parrying, no damage taken
                return;
            }
            FlashOnDamage();
        }


    }

    public void FlashOnParry()
    {
        StartFlashing(parryColor);
    }

    public void FlashOnDamage()
    {
        StartFlashing(damageColor);
    }

    private void StartFlashing(Color flashColor)
    {
        spriteRenderer.color = flashColor;
        isFlashing = true;
        flashTimer = 0f;
    }

    private void StopFlashing()
    {
        spriteRenderer.color = originalColor;
        isFlashing = false;
    }

    private void ParryCDSet()
    {
        if (parryCDTimer < parryCD)
            return;

        parryCDTimer = 0f;
        if (isFlashing)
            return;
            
        isFlashing = true;
        spriteRenderer.color = parryCDColor;
    }
}
