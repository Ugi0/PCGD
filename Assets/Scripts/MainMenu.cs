using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject mainMenuPanel;
    public GameObject mainMenuContainer;
    public GameObject creditsContainer;
    public CanvasGroup menuCanvasGroup;
    public TMP_Text highscoreText;
    public Button tutorialButton;
    public Sprite tutorialOnSprite;
    public Sprite tutorialOffSprite;
    private bool isTutorialOff;

    void Start()
    {
        isTutorialOff= (PlayerPrefs.GetInt("isTutorialOff") != 0);
        DisplayHighScore();
        AudioManager.Instance.PlayLoopingMusic("Menu");
        UpdateTutorialUI();
    }

    public void OpenCredits()
    {
        creditsContainer.SetActive(true);
        mainMenuContainer.SetActive(false);
    }

    public void CloseCredits()
    {
        mainMenuContainer.SetActive(true);
        creditsContainer.SetActive(false);
    }
    
    public void ToggleTutorial()
    {
        isTutorialOff = !isTutorialOff;
        PlayerPrefs.SetInt("isTutorialOff", isTutorialOff ? 1 : 0);
        UpdateTutorialUI();
    }

    private void UpdateTutorialUI()
    {
        if (tutorialButton == null)
        {
            Debug.LogError("Tutorial button not assigned!");
            return;
        }

        tutorialButton.image.sprite = isTutorialOff ? tutorialOffSprite : tutorialOnSprite;

        Debug.Log("Updated tutorial ui: " + isTutorialOff);
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
}
