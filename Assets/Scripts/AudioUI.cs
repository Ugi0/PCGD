using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [Header("Audio Settings")]
    public Button musicButton;
    public Button sfxButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;
    private bool isMusicMuted;
    private bool isSFXMuted;

    void Start()
    {
        // Load audio settings
        isMusicMuted = AudioManager.Instance.musicSource.mute;
        isSFXMuted = AudioManager.Instance.sfxSource.mute;

        UpdateAudioUI();   
    }

    
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        AudioManager.Instance.musicSource.mute = isMusicMuted;  

        UpdateAudioUI();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.PlaySFX("HitGlass");
        isSFXMuted = !isSFXMuted;
        AudioManager.Instance.sfxSource.mute = isSFXMuted; 
        AudioManager.Instance.loopingsfxSource.mute = isSFXMuted;

        UpdateAudioUI();
    }

    private void UpdateAudioUI()
    {
        if (musicButton == null || sfxButton == null)
        {
            Debug.LogError("Music or SFX button not assigned!");
            return;
        }

        musicButton.image.sprite = isMusicMuted ? musicOffSprite : musicOnSprite;
        sfxButton.image.sprite = isSFXMuted ? sfxOffSprite : sfxOnSprite;

        Debug.Log("Updated Audio UI - Music: " + isMusicMuted + " | SFX: " + isSFXMuted);
    }
}
