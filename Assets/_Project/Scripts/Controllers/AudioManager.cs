using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip cardFlip;
    [SerializeField] private AudioClip cardFlipBack;
    [SerializeField] private AudioClip cardMatch;
    [SerializeField] private AudioClip cardMismatch;
    
    private AudioSource audioSource;
    
    private bool allowMusic = true;
    private bool allowSFX = true;
    
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (allowMusic)
        {
            audioSource.Play();
        }
    }

    public void SetMuiscVolume(float volume)
    {
        musicVolume = volume;
        audioSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
    
    public void PlayButtonClickAudio()
    {
        PlaySFX(buttonClick);
    }

    public void PlayCardFlipAudio()
    {
        PlaySFX(cardFlip);
    }

    public void PlayCardFlipBackAudio()
    {
        PlaySFX(cardFlipBack);
    }
    public void PlayCardMatchAudio()
    {
        PlaySFX(cardMatch);
    }
    public void PlayCardMismatchAudio()
    {
        PlaySFX(cardMismatch);
    }
    
    private void PlaySFX(AudioClip clip)
    {
        if (allowSFX)
        {
            audioSource.PlayOneShot(clip, sfxVolume);
        }
    }
    
    
}
