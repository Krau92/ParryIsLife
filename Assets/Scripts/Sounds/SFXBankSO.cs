using UnityEngine;
using System;
using System.Collections.Generic;


public enum SFXType
{
    PlayerShootTail,
    PlayerShootHit,
    PlayerChargedShootHit,
    BossHit

}

[Serializable]
public class AudioRegister
{
    public SFXType sfxType;
    public AudioClip audioClip;
    [Range(0f, 1f)] public float volumeAdjustment = 1f;
    public float pitchAdjustment = 1f;
}


[CreateAssetMenu(fileName = "AudioBankSO", menuName = "Scriptable Objects/AudioBankSO")]
public class SFXBankSO : ScriptableObject
{
    [SerializeField] private List<AudioRegister> audioRegisters = new List<AudioRegister>();

    private Dictionary<SFXType, AudioRegister> audioRegisterDict;

    public void Initialize()
    {
        audioRegisterDict = new Dictionary<SFXType, AudioRegister>();

        foreach (AudioRegister register in audioRegisters)
        {
            audioRegisterDict.Add(register.sfxType, register);
        }
    }

    public AudioRegister GetAudioClip(SFXType sfxType)
    {
        if (audioRegisterDict.TryGetValue(sfxType, out AudioRegister register))
        {
            return register;
        }
        else
        {
            Debug.LogWarning($"SFXBankSO: SFXType {sfxType} not found in audio register.");
            return null;
        }
    }
}