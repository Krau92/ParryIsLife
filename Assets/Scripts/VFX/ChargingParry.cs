using UnityEngine;
using System.Collections;

public class ChargingParry : MonoBehaviour
{
    Material defaultMaterial;
    [SerializeField] Material flashingMaterial;
    [SerializeField] LineRenderer parryRenderer;

    [Tooltip("Number of vertices for the parry circle. Higher values will create a smoother circle but may impact performance.")]
    [SerializeField] private int vertexCount = 50;
    [SerializeField] private float initialRadius = 4.0f;
    [Range(0.01f, 0.3f)] [SerializeField] private float thickness = 0.1f;
    [SerializeField] private float circleContractionSmoothness = 1.5f; // Higher values will make the circle contract faster at the beginning and slower towards the end

    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private ChargingParryEffect sound;

    [SerializeField]private PlayerParry playerParry;
    private Material runtimeMaterial;
    float chargedPercentage;
    bool isChargingParry = false;
    bool fullyCharged = false;
    [SerializeField] private float flashingDuration = 0.5f;

    float flashingTimer;
    bool isFlashingStateOn; // true = runtimeMaterial (short), false = defaultMaterial (long)

    void Awake()
    {
        defaultMaterial = parryRenderer.material;
        runtimeMaterial = new Material(flashingMaterial);
        parryRenderer.positionCount = vertexCount;
        parryRenderer.loop = true;
        parryRenderer.startWidth = thickness;
        parryRenderer.endWidth = thickness;
        DrawCircle(initialRadius);
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
        parryRenderer.enabled = true;
        isChargingParry = true;
        chargedPercentage = 0f;
    }

    private void HandleChargingParryEnd()
    {
        DrawCircle(initialRadius);
        parryRenderer.enabled = false;
        isChargingParry = false;
        chargedPercentage = 0f;
        fullyCharged = false;
        playerRenderer.material = defaultMaterial;
    }

    void Update()
    {
        if (isChargingParry && !fullyCharged)
        {
            chargedPercentage = playerParry.GetChargePercentage();
            
            float radius = initialRadius * Mathf.Pow(1f - chargedPercentage, circleContractionSmoothness);
            DrawCircle(radius);
            Debug.Log("Drawing circle with radius: " + radius);
            
            if (chargedPercentage >= 1f && !fullyCharged)
            {
                fullyCharged = true;
                
                flashingTimer = 0f;
                isFlashingStateOn = false;
                playerRenderer.material = defaultMaterial;
                parryRenderer.enabled = false;
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

    private void DrawCircle(float radius)
    {
        for (int i = 0; i < vertexCount; i++)
        {
            float angle = i * 2f * Mathf.PI / vertexCount;
            float x = Mathf.Cos(angle) * radius + transform.position.x;
            float y = Mathf.Sin(angle) * radius + transform.position.y;
            parryRenderer.SetPosition(i, new Vector3(x, y, 0f));
        }
    }
}
