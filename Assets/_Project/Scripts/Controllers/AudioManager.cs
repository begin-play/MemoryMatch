
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip cardFlip;
  
    [SerializeField] private AudioClip cardMatch;
    [SerializeField] private AudioClip cardMismatch;
    
    private AudioSource audioSource;
    
    private bool allowMusic = true;
    private bool allowSfx = true;
    
    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SaveManager.Instance.LoadMusicSettings(ref musicVolume,ref sfxVolume,ref allowMusic,ref allowSfx);
       
    }
    
    private void Start()
    {
        if (allowMusic)
        {
            audioSource.Play();
        }

    }
    
    #region Button Press Events
    public void MusicTogglePressReceived(bool isOn)
    {
        allowMusic = isOn;
        if (allowMusic)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
    public void SfxTogglePressReceived(bool isOn)
    {
        allowSfx = isOn;
    }
    public void MusicVolumeChangeReceived(float value)
    {
        musicVolume = value;
        audioSource.volume = musicVolume;
    }
    public void SfxVolumeChangeReceived(float value)
    {
        sfxVolume = value;
    }
    
   #endregion
    
    #region Play Audio
    public void PlayButtonClickAudio()
    {
        PlaySfx(buttonClick);
    }

    public void PlayCardFlipAudio()
    {
        PlaySfx(cardFlip);
    }
    public void PlayCardMatchAudio()
    {
        PlaySfx(cardMatch);
    }
    public void PlayCardMismatchAudio()
    {
        PlaySfx(cardMismatch);
    }
    
    private void PlaySfx(AudioClip clip)
    {
        if (allowSfx)
        {
            audioSource.PlayOneShot(clip, sfxVolume);
        }
    }
    #endregion
    
}
