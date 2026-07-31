using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MenuPanel
{
    Button button;
    public AudioMixerGroup music, sfx;

    public void MusicVolumeUpdated(float value)
    {
        music.audioMixer.SetFloat("MusicVolume", PercentageToDecibels(value));
    }

    public void SFXVolumeUpdated(float value)
    {
        sfx.audioMixer.SetFloat("SFXVolume", PercentageToDecibels(value));
    }

    public float PercentageToDecibels(float value)
    {
        return Mathf.Log10(value) * 20;
    }

}
