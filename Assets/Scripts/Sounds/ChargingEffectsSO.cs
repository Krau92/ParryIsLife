using UnityEngine;
using System;
using System.Collections.Generic;


public enum ChargingState
{
    ChargingRumble,
    ChargingRaising,
    FullyCharged

}

[Serializable]
public class ChargingRegister
{
    public ChargingState chargingState;
    public AudioClip audioClip;
    [Range(0f, 1f)] public float volumeAdjustment = 1f;
    public float initialPitchAdjustment = 1f;
    public float finalPitchAdjustment = 1f;
}


[CreateAssetMenu(fileName = "ChargingEffectsSO", menuName = "Scriptable Objects/ChargingEffectsSO")]
public class ChargingEffectsSO : ScriptableObject
{
    [SerializeField] private List<ChargingRegister> chargingRegisters = new List<ChargingRegister>();

    private Dictionary<ChargingState, ChargingRegister> chargingRegisterDict;

    public void Initialize()
    {
        chargingRegisterDict = new Dictionary<ChargingState, ChargingRegister>();

        foreach (ChargingRegister register in chargingRegisters)
        {
            chargingRegisterDict.Add(register.chargingState, register);
        }
    }

    public ChargingRegister GetAudioClip(ChargingState chargingState)
    {
        if (chargingRegisterDict.TryGetValue(chargingState, out ChargingRegister register))
        {
            return register;
        }
        else
        {
            Debug.LogWarning($"ChargingEffectsSO: ChargingState {chargingState} not found in charging register.");
            return null;
        }
    }
}