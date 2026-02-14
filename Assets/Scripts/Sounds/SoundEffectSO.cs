using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class AudioRegister
{
    public AudioClip audioClip;
    [Range(0f, 1f)] public float volumeAdjustment = 1f;
    public float pitchAdjustment = 1f;
}


[CreateAssetMenu(fileName = "SoundEffectSO", menuName = "Audio/SoundEffect")]
public class SoundEffectSO : ScriptableObject
{
    [SerializeField] private List<AudioRegister> audioRegisters = new List<AudioRegister>();
    [SerializeField] private float pitchRandomRange = 0.15f;
    [SerializeField][Range(0f, 1f)] private float volumeRandomRange = 0.1f;

    //! If I want to implement multiple audio system with looped sounds, activate again the list
    // private List<PooledAudioSource> playingAudioResources = new List<PooledAudioSource>();


    private static PooledAudioSource cachedAudioPrefab;

    public void PlayEffect()
    {

        // Load prefab from Resources if not cached
        if (cachedAudioPrefab == null)
        {
            GameObject prefabObj = Resources.Load<GameObject>("Prefabs/SFXAudioSource");
            cachedAudioPrefab = prefabObj.GetComponent<PooledAudioSource>();
        }

        if (cachedAudioPrefab == null)
        {
            Debug.LogError("PooledAudioSource prefab not found in Resources/Prefabs/SFXAudioSource");
            return;
        }

        foreach (AudioRegister register in audioRegisters)
        {
            PooledAudioSource pooledAudio = PoolManager.SpawnObject<PooledAudioSource>(cachedAudioPrefab, Vector3.zero, Quaternion.identity, PoolManager.PoolType.Audio);
            if (pooledAudio != null)
            {
                float randomPitch = UnityEngine.Random.Range(-pitchRandomRange, pitchRandomRange);
                float finalPitch = register.pitchAdjustment + randomPitch;

                float randomVolume = UnityEngine.Random.Range(-volumeRandomRange, volumeRandomRange);
                float finalVolume = Mathf.Clamp01(register.volumeAdjustment + randomVolume);

                pooledAudio.Play(register.audioClip, finalVolume, finalPitch);
            }
        }


    }

}