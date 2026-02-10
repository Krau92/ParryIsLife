using UnityEngine;
using System.Collections;

public class PooledAudioSource : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void RotatePitch(float pitch)
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

        StartCoroutine(WaitForAudioToEnd());
    }

    private IEnumerator WaitForAudioToEnd()
    {
        if(audioSource.clip == null)
        {
             PoolManager.ReturnObjectToPool(this.gameObject, PoolManager.PoolType.Audio);
             yield break;
        }

        // Wait for the length of the clip, adjusted for pitch
        // Ensure pitch is not zero to avoid division by zero
        float pitch = Mathf.Abs(audioSource.pitch);
        if (pitch < 0.01f) pitch = 1f;

        yield return new WaitForSeconds(audioSource.clip.length / pitch);

        PoolManager.ReturnObjectToPool(this.gameObject, PoolManager.PoolType.Audio);
    }
}
