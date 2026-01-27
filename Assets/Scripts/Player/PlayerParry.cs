using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    [Header("Parry Settings")]
    [SerializeField] private float parryDuration = 0.2f;
    [SerializeField] private float parryCooldown = 1f;
    [SerializeField] private float chargedParryIncreaseDuration = 0.3f;
    [SerializeField] private float chargedParryHoldTime = 1.5f;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;

    //Control Variables
    private float lastParryTime;
    private float parryHoldTimer = 0f;
    public bool IsParrying{ get; private set; } 

    void OnEnable()
    {
        InputManager.onParryInput += TryToParry;
        InputManager.onStopParryInput += TryToChargedParry;

    }

    void OnDisable()
    {
        InputManager.onParryInput -= TryToParry;
        InputManager.onStopParryInput -= TryToChargedParry;
    }


    void Update()
    {
        HandleParry();

        //Manage the feedback for charged parry hold time
    }

    //Method to handle parry timing and state
    private void HandleParry()
    {
        if(InputManager.Instance.IsParryHold())
        {
            parryHoldTimer += Time.deltaTime;
            parryHoldTimer = Mathf.Clamp(parryHoldTimer, 0f, chargedParryHoldTime);
        }
        else
            parryHoldTimer = 0f;

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

            //TODO: Manager returning projectiles. Maybe create new bullet coloured?
        }
    }
}
