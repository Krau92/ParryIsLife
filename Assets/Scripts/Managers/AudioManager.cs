using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SFXBankSO sfxBank;
    [SerializeField] private PooledAudioSource audioSourcePrefab;
    [SerializeField] private float randomPitchRange = 0.15f;
    [SerializeField] private float soundCD = 0.5f;

    private float nextSoundTime = 0f;

    void Start()
    {
        sfxBank.Initialize();
    }

    void OnEnable()
    {
        CombatEvents.OnPlayerShoot += HandlePlayerShoot;
        CombatEvents.OnBossDamaged += HandleEnemyHit;
    }

    void OnDisable()
    {
        CombatEvents.OnPlayerShoot -= HandlePlayerShoot;
        CombatEvents.OnBossDamaged -= HandleEnemyHit;
    }

    private void PlaySFX(SFXType sfxType)
    {
        AudioRegister register = sfxBank.GetAudioClip(sfxType);
        if (register != null)
        {
            PooledAudioSource source = PoolManager.SpawnObject(audioSourcePrefab, transform.position, Quaternion.identity, PoolManager.PoolType.Audio);
            
            if (source != null)
            {
                source.Play(register.audioClip, register.volumeAdjustment, register.pitchAdjustment);
            }
        }
    }

    private void PlaySFX(SFXType sfxType, float volumeAdjustment, float pitchAdjustment)
    {
        AudioRegister register = sfxBank.GetAudioClip(sfxType);
        if (register != null)
        {
            PooledAudioSource source = PoolManager.SpawnObject(audioSourcePrefab, transform.position, Quaternion.identity, PoolManager.PoolType.Audio);

            if (source != null)
            {
                source.Play(register.audioClip, 
                          register.volumeAdjustment * volumeAdjustment, 
                          register.pitchAdjustment * pitchAdjustment);
            }
        }
    }

    private void HandlePlayerShoot(bool isChargedShot)
    {
        if (Time.time < nextSoundTime)        
        {
            return;
        }


        float randomPitchMultiplier = Random.Range(1f - randomPitchRange, 1f + randomPitchRange);
        PlaySFX(SFXType.PlayerShootTail, 1f, randomPitchMultiplier);
        if (isChargedShot)
        {
            nextSoundTime = Time.time + soundCD;
            PlaySFX(SFXType.PlayerChargedShootHit, 1f, randomPitchMultiplier);
        }
        else
        {
            PlaySFX(SFXType.PlayerShootHit, 1f, randomPitchMultiplier);
        }
    }
    
    private void HandleEnemyHit()
    {
        float randomPitchMultiplier = Random.Range(1f - randomPitchRange, 1f + randomPitchRange);
        PlaySFX(SFXType.BossHit, 1f, randomPitchMultiplier);
    }
}
