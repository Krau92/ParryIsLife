using UnityEngine;
using System.Collections;

public class ChargingSootEffect : MonoBehaviour
{
    [SerializeField] private ChargingEffectsSO chargingEffectsSO;
    float initialPitch;
    float targetPitch;
    float finalVolume;
    float percentage;

    bool isCharging = false;
    bool completelyCharged = false;
    
    [SerializeField]PlayerShoot playerShoot;
    [SerializeField] AudioSource rumbleAudioSource;
    [SerializeField] AudioSource pitchShiftAudioSource;
    [SerializeField] AudioSource fullyChargedAudioSource;

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
            pitchShiftAudioSource.volume = pitchRegister.volumeAdjustment;
            initialPitch = pitchRegister.initialPitchAdjustment;
            targetPitch = pitchRegister.finalPitchAdjustment;
        }

        ChargingRegister fullyChargedRegister = chargingEffectsSO.GetAudioClip(ChargingState.FullyCharged);
        if (fullyChargedRegister != null)
        {
            fullyChargedAudioSource.loop = true;
            fullyChargedAudioSource.clip = fullyChargedRegister.audioClip;
            fullyChargedAudioSource.pitch = fullyChargedRegister.initialPitchAdjustment;
            fullyChargedAudioSource.volume = fullyChargedRegister.volumeAdjustment;
        }

        isCharging = false;
        completelyCharged = false;
        percentage = 0f;

    }

    void OnEnable()
    {
        CombatEvents.OnChargingShotStart += HandleChargingShotStart;
        CombatEvents.OnChargingShotEnd += HandleChargingShotEnd;
    }

    void OnDisable()
    {
        CombatEvents.OnChargingShotStart -= HandleChargingShotStart;
        CombatEvents.OnChargingShotEnd -= HandleChargingShotEnd;
    }

    void Update()
    {
        if (isCharging && !completelyCharged)
        {
            if(percentage >= 1f)
            {
                percentage = 1f;
                fullyChargedAudioSource.Play();
                completelyCharged = true;
            }

            percentage = playerShoot.GetChargePercentage();
            pitchShiftAudioSource.pitch = Mathf.Lerp(initialPitch, targetPitch, percentage);
            rumbleAudioSource.volume = Mathf.Lerp(0f, finalVolume, percentage);
        }
    }

    private void HandleChargingShotStart()
    {
        rumbleAudioSource.Play();
        pitchShiftAudioSource.Play();
        isCharging = true;
        percentage = 0f;

    }

    private void HandleChargingShotEnd()
    {
        isCharging = false;
        completelyCharged = false;
        rumbleAudioSource.Stop();
        pitchShiftAudioSource.Stop();
        StopChargeAudio();

        if(percentage >= 1f)
        {
            CombatEvents.OnPlayerShoot.Invoke(true); // Volumen aumentado para el disparo cargado
        }
    }

    // Variable para guardar la corrutina actual y poder interrumpirla
private Coroutine fadeRoutine;

public void StopChargeAudio()
{
    if (fadeRoutine != null) StopCoroutine(fadeRoutine);
    fadeRoutine = StartCoroutine(FadeOutAndStop(fullyChargedAudioSource, 0.15f)); // 0.15s es suave y rápido
}

private IEnumerator FadeOutAndStop(AudioSource source, float duration)
{
    float startVolume = source.volume;
    float time = 0;

    while (time < duration)
    {
        time += Time.deltaTime;
        // Interpolamos el volumen hacia 0
        source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
        yield return null;
    }

    source.Stop(); // Ahora sí, paramos el motor
    source.volume = startVolume; // (Opcional) Restaurar volumen para la próxima vez
}

}
