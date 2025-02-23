using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject mainMenuPanel;
    public CanvasGroup menuCanvasGroup;
    public TMP_Text highscoreText;

    [Header("Audio Settings")]
    public Button musicButton;
    public Button sfxButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;

    private AudioManager audioManager;
    private bool isMusicMuted;
    private bool isSFXMuted;

    void Start()
    {

        DisplayHighScore();
        // Find AudioManager
        audioManager = AudioManager.Instance;

        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in scene!");
            return;
        }

        // Load audio settings
        isMusicMuted = audioManager.musicSource.mute;
        isSFXMuted = audioManager.sfxSource.mute;

        UpdateAudioUI();
    }

    private void DisplayHighScore()
    {
        int highscore = PlayerPrefs.GetInt("highscore", 0);
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }

    public void PlayGame()
    {
        Debug.Log("Play button pressed.");
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        float duration = 0.5f;
        float time = 0;

        menuCanvasGroup.blocksRaycasts = false;
        menuCanvasGroup.interactable = false;

        while (time < duration)
        {
            menuCanvasGroup.alpha = Mathf.Lerp(1, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        menuCanvasGroup.alpha = 0;
        mainMenuPanel.SetActive(false);

        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    public void ToggleMusic()
    {
        if (audioManager == null) return;

        isMusicMuted = !isMusicMuted;
        audioManager.musicSource.mute = isMusicMuted;  

        UpdateAudioUI();
    }

    public void ToggleSFX()
    {
        if (audioManager == null) return;

        isSFXMuted = !isSFXMuted;
        audioManager.sfxSource.mute = isSFXMuted; 
        audioManager.loopingsfxSource.mute = isSFXMuted;

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
