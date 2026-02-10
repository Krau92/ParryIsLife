using UnityEngine;
using System.Collections;

public class ChargingParryEffect : MonoBehaviour
{
    [SerializeField] private ChargingEffectsSO chargingEffectsSO;
    float initialPitch;
    float targetPitch;
    float finalVolume;
    float percentage;

    bool isCharging = false;
    bool completelyCharged = false;

    [SerializeField] PlayerParry playerParry;
    [SerializeField] AudioSource rumbleAudioSource;
    [SerializeField] AudioSource pitchShiftAudioSource;
    [SerializeField] AudioSource fullyChargedAudioSource;
    [SerializeField] private float expValue = 2f;

    private void Awake()
    {
        chargingEffectsSO.Initialize();
        ChargingRegister register = chargingEffectsSO.GetAudioClip(ChargingState.ChargingRumble);
        if (register != null)
        {
            rumbleAudioSource.loop = true;
            rumbleAudioSource.clip = register.audioClip;
            rumbleAudioSource.volume = 0f;
            finalVolume = register.volumeAdjustment;
        }

        ChargingRegister pitchRegister = chargingEffectsSO.GetAudioClip(ChargingState.ChargingRaising);
        if (pitchRegister != null)
        {
            pitchShiftAudioSource.loop = true;
            pitchShiftAudioSource.clip = pitchRegister.audioClip;
            pitchShiftAudioSource.pitch = pitchRegister.initialPitchAdjustment;
            initialPitch = pitchRegister.initialPitchAdjustment;
            targetPitch = pitchRegister.finalPitchAdjustment;
            pitchShiftAudioSource.volume = pitchRegister.volumeAdjustment;
        }

        ChargingRegister fullyChargedRegister = chargingEffectsSO.GetAudioClip(ChargingState.FullyCharged);
        if (fullyChargedRegister != null)
        {
            fullyChargedAudioSource.clip = fullyChargedRegister.audioClip;
            fullyChargedAudioSource.pitch = fullyChargedRegister.initialPitchAdjustment;
            fullyChargedAudioSource.volume = fullyChargedRegister.volumeAdjustment;
        }

        isCharging = false;
        percentage = 0f;
        completelyCharged = false;

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

    void Update()
    {
        if (isCharging && !completelyCharged)
        {
            if (percentage >= 1f)
            {
                percentage = 1f;
                // rumbleAudioSource.Stop();
                pitchShiftAudioSource.Stop();
                fullyChargedAudioSource.Play();
                completelyCharged = true;
            }

            percentage = playerParry.GetChargePercentage();

            CalculateSoundEffect();
        }
    }

    private void HandleChargingParryStart()
    {
        isCharging = true;
        percentage = 0f;
        CalculateSoundEffect();

        // rumbleAudioSource.Play();
        pitchShiftAudioSource.Play();
    }

    private void HandleChargingParryEnd()
    {
        isCharging = false;
        completelyCharged = false;
        // rumbleAudioSource.Stop();
        pitchShiftAudioSource.Stop();
        fullyChargedAudioSource.Stop();
    }

    private void CalculateSoundEffect()
    {
        // rumbleAudioSource.volume = Mathf.Lerp(0f, finalVolume, percentage);

        percentage = Mathf.Pow(percentage, expValue);
        pitchShiftAudioSource.pitch = Mathf.Lerp(initialPitch, targetPitch, percentage);
    }

    public void PlayFullyChargedSound()
    {
        // rumbleAudioSource.pitch = initialPitch;
        rumbleAudioSource.volume = finalVolume;
        rumbleAudioSource.PlayOneShot(rumbleAudioSource.clip);
    }


}
