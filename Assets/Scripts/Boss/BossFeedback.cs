using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossFeedback : MonoBehaviour
{
    private Material defaultMaterial;
    private Boss boss;
    [SerializeField] private Material hitMaterial;
    [SerializeField] private Gradient hitGradient;
    [SerializeField] private float hitFlashDuration = 0.05f;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Material runtimeHitMaterial;
    private Coroutine flashCoroutine;

    void Awake()
    {
        boss = GetComponent<Boss>();
        Debug.Log("Boss script found? " + (boss != null));

        runtimeHitMaterial = new Material(hitMaterial);
        defaultMaterial = spriteRenderer.material; 
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