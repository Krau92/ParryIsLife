using UnityEngine;
using System.Collections;

public class PooledAudioSource : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isPlaying = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if(isPlaying)
        {
            if(audioSource.isPlaying == false)
            {
                DeactivateObject();
            }
        }
    }

    public void ModifyPitch(float pitch)
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.pitch = pitch;
    }

    public void Play(AudioClip clip, float volumeEffect, float pitchEffect)
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volumeEffect;
        audioSource.pitch = pitchEffect;
        
        audioSource.Play();

        isPlaying = true;
    }

    //Stopping audio externally in case I want to implement looped sounds or music
    public void Stop()
    {
        
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        DeactivateObject();
    }

    private void DeactivateObject()
    {
        isPlaying = false;
        PoolManager.ReturnObjectToPool(gameObject, PoolManager.PoolType.Audio);
    }

    
}
