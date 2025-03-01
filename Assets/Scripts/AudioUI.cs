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
        isMusicMuted = (PlayerPrefs.GetInt("isMusicMuted") != 0);
        isSFXMuted = (PlayerPrefs.GetInt("isSFXMuted") != 0);
        AudioManager.Instance.sfxSource.mute = isSFXMuted;
        AudioManager.Instance.loopingsfxSource.mute = isSFXMuted;
        AudioManager.Instance.musicSource.mute = isMusicMuted;
        UpdateAudioUI();   
    }

    
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        AudioManager.Instance.musicSource.mute = isMusicMuted;
        PlayerPrefs.SetInt("isMusicMuted", isMusicMuted ? 1 : 0);
        UpdateAudioUI();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.PlaySFX("HitGlass");
        isSFXMuted = !isSFXMuted;
        AudioManager.Instance.sfxSource.mute = isSFXMuted; 
        AudioManager.Instance.loopingsfxSource.mute = isSFXMuted;
        PlayerPrefs.SetInt("isSFXMuted", isSFXMuted ? 1 : 0);
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
