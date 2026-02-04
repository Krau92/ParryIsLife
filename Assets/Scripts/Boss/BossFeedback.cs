using UnityEngine;
using System.Collections;

public class BossFeedback : MonoBehaviour
{
    private Material defaultMaterial;
    private Boss boss;
    [SerializeField] private Material hitMaterial;
    [SerializeField] private Gradient hitGradient;
    [SerializeField] private float hitFlashDuration = 0.05f;

    private SpriteRenderer spriteRenderer;
    private Material runtimeHitMaterial;
    private Coroutine flashCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boss = GetComponent<Boss>();
        Debug.Log("Boss script found? " + (boss != null));

        defaultMaterial = spriteRenderer.material;
        runtimeHitMaterial = new Material(hitMaterial);
    }

    void OnEnable()
    {
        CombatEvents.OnBossDamaged += OnBossDamaged;
    }

    void OnDisable()
    {
        CombatEvents.OnBossDamaged -= OnBossDamaged;
    }

    void OnDestroy()
    {
        if (runtimeHitMaterial != null)
        {
            Destroy(runtimeHitMaterial);
        }
    }

    private void OnBossDamaged()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashHitMaterial());
    }

    private IEnumerator FlashHitMaterial()
    {
        Color bossHealthColor = hitGradient.Evaluate(1.0f - boss.GetHealthPercentage());
        runtimeHitMaterial.SetColor("_Color", bossHealthColor);
        spriteRenderer.material = runtimeHitMaterial;

        yield return new WaitForSeconds(hitFlashDuration);

        spriteRenderer.material = defaultMaterial;
        flashCoroutine = null;
    }
}