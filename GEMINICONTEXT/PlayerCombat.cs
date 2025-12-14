using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Parry Settings")]
    [SerializeField] private float parryDuration = 0.2f;
    [SerializeField] private float parryCooldown = 1f;
    [SerializeField] private float chargedParryIncreaseDuration = 0.3f;

    //Control Variables
    private float lastParryTime;
    public bool IsParrying{ get; private set; } 

    void Update()
    {
        HandleParry();
    }

    //Method to initiate parry action (trigger parry animation + set flag to true)
    public void TryToParry()
    {
        if (Time.time >= lastParryTime + parryCooldown)
        {
            IsParrying = true;
            lastParryTime = Time.time;

            //TODO: Trigger parry animation

        }
    }

    //Method to handle parry timing and state
    private void HandleParry()
    {
        if (IsParrying)
            if (Time.time >= lastParryTime + parryDuration)
                StopParry();
    }

    //Method to manage the end of parry (call the stop parry event + marking the flag as false)
    private void StopParry()
    {
        IsParrying = false;

        //TODO: Call stop parry event

    }
}
