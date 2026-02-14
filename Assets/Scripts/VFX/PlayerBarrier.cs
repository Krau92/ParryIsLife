using Unity.VisualScripting;
using UnityEngine;

public class PlayerBarrier : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    private SpriteRenderer spriteRenderer;

    [Header("Color Settings")]
    [ColorUsage(true, true)] [SerializeField] private Color barrierColor = new Color(0.5f, 0.5f, 1f, 0.5f);
    [ColorUsage(true, true)] [SerializeField] private Color damagedColor = new Color(0.5f, 0.5f, 1f, 0.5f);
    [ColorUsage(true, true)] [SerializeField] private Color hitColor = new Color(1f, 0.5f, 0.5f, 0.8f);
    [SerializeField] private float hitFlashDuration = 0.2f;
    [SerializeField] private float damageFlashDuration = 0.5f;

    [ColorUsage(true, true)] [SerializeField] private Color deactivatedColor = new Color(0.5f, 0.5f, 1f, 0f);
    private Color currentBarrierColor;
    private bool isHit = false;
    private bool flashingDamage = false;
    private float hitTimer = 0f;
    private float flashTimer = 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("PlayerBarrier requires a SpriteRenderer component.");
            enabled = false;
            return;
        }
        SetBarrierColor(barrierColor);
    }

    private void OnEnable()
    {
        CombatEvents.OnBossSelected += ResetBarrierColor;
        CombatEvents.OnDamageTaken += UpdateBarrierColor;
    }

    private void OnDisable()
    {
        CombatEvents.OnBossSelected -= ResetBarrierColor;
        CombatEvents.OnDamageTaken -= UpdateBarrierColor;
    }

    private void Update()
    {
        if (isHit)
        {
            if(Time.time >= hitTimer)
            {
                isHit = false;
                flashingDamage = false;
                SetBarrierColor(currentBarrierColor);
                return;
            }
            if(Time.time >= flashTimer)
            {
                flashTimer = Time.time + hitFlashDuration;
                SetBarrierColor(flashingDamage ? damagedColor : hitColor);
                flashingDamage = !flashingDamage;
            }
        }
    }

    private void ResetBarrierColor(Boss boss)
    {
        SetBarrierColor(barrierColor);
    }

    private void UpdateBarrierColor()
    {

        switch (playerHealth.CurrentHealth)
        {
            case 3:
                currentBarrierColor = barrierColor;
                break;

            case 2:
                currentBarrierColor = damagedColor;
                break;

            case 1:
                currentBarrierColor = deactivatedColor;
                break;

            default:
                currentBarrierColor = deactivatedColor;
            return;
        }

        SetBarrierColor(currentBarrierColor);
        isHit = true;
        flashingDamage = true;
        hitTimer = Time.time + damageFlashDuration;
        flashTimer = Time.time + hitFlashDuration;
    }

    private void SetBarrierColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.material.SetColor("_BarrierColor", color);
        }
    }
}