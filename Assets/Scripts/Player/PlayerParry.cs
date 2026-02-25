using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    [Header("Parry Settings")]
    [SerializeField] private float parryDuration = 0.2f;
    [SerializeField] private float parryCooldown = 1f;
    [SerializeField] private float chargedParryIncreaseDuration = 0.3f;
    [SerializeField] private float chargedParryHoldTime = 1.5f;
    [SerializeField] private float chargingBufferTime = 0.2f;

    public float GetChargePercentage() {  return parryHoldTimer / chargedParryHoldTime; }

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;

    //Control Variables
    private bool isChargingParry = false;
    private float chargingBufferTimer;
    private float lastParryTime;
    private float parryHoldTimer = 0f;
    public bool IsParrying{ get; private set; } 

    void OnEnable()
    {
        isChargingParry = false;
        InputManager.onParryInput += TryToParry;
        InputManager.onStopParryInput += TryToChargedParry;
        CombatEvents.OnCombatEnded += TryToChargedParry; 

    }

    void OnDisable()
    {
        InputManager.onParryInput -= TryToParry;
        InputManager.onStopParryInput -= TryToChargedParry;
        CombatEvents.OnCombatEnded -= TryToChargedParry;
    }


    void Update()
    {
        HandleParry();

    }

    //Method to handle parry timing and state
    private void HandleParry()
    {
        if(InputManager.Instance.IsParryHold())
        {
            if(!isChargingParry)
            {
                chargingBufferTimer += Time.deltaTime;
                if(chargingBufferTimer >= chargingBufferTime)
                {
                    isChargingParry = true;
                    CombatEvents.OnChargingParryStart?.Invoke();
                }
                return;
            }
            parryHoldTimer += Time.deltaTime;
            parryHoldTimer = Mathf.Clamp(parryHoldTimer, 0f, chargedParryHoldTime);
        }
        else
        {
            chargingBufferTimer = 0f;
            isChargingParry = false;
            CombatEvents.OnChargingParryEnd?.Invoke();
            parryHoldTimer = 0f;
        }

        if (IsParrying)
            if (Time.time >= lastParryTime + parryDuration)
                StopParry();
    }   

    //Method to initiate parry action (trigger parry animation + set flag to true)
    public void TryToParry()
    {
        playerMovement.SetSlowed(true);
        if (Time.time >= lastParryTime + parryCooldown)
        {
            IsParrying = true;
            lastParryTime = Time.time;

            playerHealth.SetParrying(parryDuration);

            //TODO: Trigger parry animation

        }
    }

    //Method to manage the end of parry (call the stop parry event + marking the flag as false)
    private void StopParry()
    {
        IsParrying = false;
    }

    private void TryToChargedParry()
    {
        playerMovement.SetSlowed(false);

        if (parryHoldTimer >= chargedParryHoldTime)
        {
            IsParrying = true;
            lastParryTime = Time.time;

            playerHealth.SetParrying(parryDuration + chargedParryIncreaseDuration);
            playerHealth.SetReflecting(parryDuration + chargedParryIncreaseDuration);
        }
    }

}
