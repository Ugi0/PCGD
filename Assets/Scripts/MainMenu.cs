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

    void Start()
    {

        DisplayHighScore();
        AudioManager.Instance.PlayLoopingMusic("Menu");

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
