using UnityEngine;
using System.Collections;

public class ChargingParry : MonoBehaviour
{
    Material defaultMaterial;
    [SerializeField] Material flashingMaterial;
    [SerializeField] SpriteRenderer parryRenderer;
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private ChargingParryEffect sound;
    private PlayerParry playerParry;
    private Material runtimeMaterial;
    float chargedPercentage;
    bool isChargingParry = false;
    bool fullyCharged = false;
    float initialScale;
    [SerializeField] private float flashingDuration = 0.5f;

    float flashingTimer;
    bool isFlashingStateOn; // true = runtimeMaterial (short), false = defaultMaterial (long)

    void Awake()
    {
        playerParry = GetComponent<PlayerParry>();
        defaultMaterial = parryRenderer.material;
        runtimeMaterial = new Material(flashingMaterial);
        initialScale = parryRenderer.transform.localScale.x;
    }


    void OnEnable()
    {
        CombatEvents.OnChargingParryStart += HandleChargingParryStart;
        CombatEvents.OnChargingParryEnd += HandleChargingParryEnd;
    }
    
    void OnDisable()
    {
        CombatEvents.OnChargingParryStart -= HandleChargingParryStart;
        CombatEvents.OnChargingParryEnd -= HandleChargingParryEnd;
    }

    private void HandleChargingParryStart()
    {
        isChargingParry = true;
        chargedPercentage = 0f;
    }

    private void HandleChargingParryEnd()
    {
        isChargingParry = false;
        chargedPercentage = 0f;
        fullyCharged = false;
        parryRenderer.transform.localScale = Vector3.one * initialScale;
        parryRenderer.material = defaultMaterial;
    }

    void Update()
    {
        if (isChargingParry && !fullyCharged)
        {
            parryRenderer.transform.localScale = Vector3.one * initialScale * (1f - playerParry.GetChargePercentage());
            chargedPercentage = playerParry.GetChargePercentage();
            if (chargedPercentage >= 1f && !fullyCharged)
            {
                fullyCharged = true;
                
                flashingTimer = 0f;
                isFlashingStateOn = false;
                parryRenderer.material = defaultMaterial;
            }
        }

        if (fullyCharged)
        {
            HandleFlashingEffect();
        }
    }

    private void HandleFlashingEffect()
    {
        flashingTimer += Time.deltaTime;

        if (!isFlashingStateOn) 
        {
            if (flashingTimer >= flashingDuration)
            {
                playerRenderer.material = runtimeMaterial;
                sound.PlayFullyChargedSound();
                isFlashingStateOn = true;
                flashingTimer = 0f;
            }
        }
        else 
        {
            if (flashingTimer >= flashingDuration / 4f)
            {
                
                playerRenderer.material = defaultMaterial;
                isFlashingStateOn = false;
                flashingTimer = 0f;
            }
        }
    }
}
