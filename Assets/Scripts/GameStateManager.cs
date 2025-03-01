using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using TMPro;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    
    private int Health = 3;

    public Text scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI MainMenuScoreText;
    public TextMeshProUGUI GameOverMenuPercentageText;

    public GameObject healthSpritePrefab;
    public Transform healthContainer;
    public float healthSpacing = 20f;
    public float leftPadding = 20f;
    public GameObject gameOverScreen;
    private CanvasGroup gameOverCanvas;

    public int score = 0;
    int highscore = 0;
    private Dictionary<string, Coroutine> activeDelays = new Dictionary<string, Coroutine>();
    private List<GameObject> rockIcons = new List<GameObject>();


    private void Awake() {
        instance = this;
        gameOverCanvas = gameOverScreen.GetComponent<CanvasGroup>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString();
        //highscoreText.text = "HIGHSCORE: " + highscore.ToString();

        StartMusic();
        ResetHealth();
    }
    public void Reset() {
        SceneManager.LoadScene("SampleScene");
    }
    public void ResetHealth() {
        Health = 3;
        UpdateRockDisplay(Health);
    }
    public void ReduceHealth() {
        Health -= 1;
        if (Health <= 0) {
            UpdateRockDisplay(Health);

            // Check if the player achieved a new high score
            bool isNewHighScore = score > highscore;

            // Update the game over message
            if (isNewHighScore) {
                MainMenuScoreText.text = "Score: " + score.ToString();
                highscoreText.text = "New high score!";
                PlayerPrefs.SetInt("highscore", score); // Save the new high score
            } else {
                MainMenuScoreText.text = "Score: " + score.ToString();
                highscoreText.text = "High Score: " + highscore.ToString();
            }
            float percentile = score == 0 ? 0.0f : CalculatePercentile(score);
            GameOverMenuPercentageText.text = $"YOU BEAT {percentile:F1}% OF ALL PLAYERS";
            // Game over
            // Show the Game Over screen with fade-in effect
            PlayerController.instance.SetAllowThrow(false);
            ObstacleSpawner.instance.ClearOldTargets();
            ObstacleSpawner.instance.ClearOldObstacles();
            StartCoroutine(PoliceCarManager.instance.MoveSequence());
            StartCoroutine(FadeInGameOverScreen());
        }
        UpdateRockDisplay(Health);
    }

    public void AddPoints() {
        score += 1;
        scoreText.text = score.ToString();

        if (highscore < score) {
            PlayerPrefs.SetInt("highscore", score);
        }
    }

    private void StartMusic()
    {
        AudioManager.Instance.PlayLoopingMusic("Theme");
    }

    public void HitGround() {
        ReduceHealth();
        Debug.Log(Health + " health remaining");
    }
    public void registerHit() {
        ResetHealth();
        PlayerController.instance.showOldThrow(false);
        PlayerController.instance.StartSkating();
    }

    public void StartTransition()
    {
        PlayerController.instance.ResetPlayer();
        StartDelayedAction("StartSkating", .5f, () => {
            AudioManager.Instance.PlayLoopingSFX("Skateboarding");
            PlayerController.instance.AnimateSkateboard(true);
            BackgroundManager.instance.SkatingTransition(true);
            ObstacleSpawner.instance.ClearOldObstacles();
           StopTransition();
        });
    }

    public void StopTransition()
    {
        StopDelayedAction("StartSkating");
        StartDelayedAction("StopSkating", 1f, () => {
            AudioManager.Instance.StopLoopingSFX();
            PlayerController.instance.StopSkating();
            float targetDistance = ObstacleSpawner.instance.SpawnTargets();
            ObstacleSpawner.instance.SpawnObstacles(targetDistance);
            DelayedBecomeIdle();
        });
    }

    public void DelayedBecomeIdle()
    {
        StopDelayedAction("StopSkating");
        StartDelayedAction("BecomeIdle", .6f, () => {
            PlayerController.instance.BecomeIdle();
            StopDelayedAction("BecomeIdle");
        });
    }

    public void DelayThrow()
    {
        StartDelayedAction("AllowThrow", .2f, () => {
            PlayerController.instance.SetAllowThrow(true);
        });
    }

    public void StartDelayedAction(string id, float delay, System.Action action)
    {
        if (activeDelays.ContainsKey(id))
        {
            StopCoroutine(activeDelays[id]);
            activeDelays.Remove(id);
        }

        Coroutine coroutine = StartCoroutine(DelayedActionCoroutine(id, delay, action));
        activeDelays[id] = coroutine;
    }

    public void StopDelayedAction(string id)
    {
        if (activeDelays.ContainsKey(id))
        {
            StopCoroutine(activeDelays[id]);
            activeDelays.Remove(id);
        }
    }

    private IEnumerator DelayedActionCoroutine(string id, float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        
        activeDelays.Remove(id);

        action?.Invoke();
    }

    public void UpdateRockDisplay(int rockCount)
    {
        // Remove extra rocks if necessary
        while (rockIcons.Count > rockCount)
        {
            Destroy(rockIcons[rockIcons.Count - 1]);
            rockIcons.RemoveAt(rockIcons.Count - 1);
        }

        // Add more rocks if needed
        while (rockIcons.Count < rockCount)
        {
            GameObject newRock = Instantiate(healthSpritePrefab, healthContainer);
            RectTransform rt = newRock.GetComponent<RectTransform>();

            // Set the rock's position correctly inside the container
            float startX = leftPadding + (rockIcons.Count * healthSpacing);
            rt.anchoredPosition = new Vector2(startX, 0);
            rockIcons.Add(newRock);
        }
    }

    public void RestartGame() 
    {
        StartCoroutine(FadeOutAndRestart());
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; //
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator FadeInGameOverScreen()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("End");
        AudioManager.Instance.PlaySFX("Siren");
        gameOverScreen.SetActive(true);  // Enable Game Over screen
        float duration = 1.5f;           // Fade-in duration
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            gameOverCanvas.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        gameOverCanvas.alpha = 1;
    }

    IEnumerator FadeOutAndRestart()
    {
        float duration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            gameOverCanvas.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        gameOverCanvas.alpha = 0;
        gameOverScreen.SetActive(false);
        Time.timeScale = 1f; // Resume game
        Reset();
    }

    float CalculatePercentile(float score)
    {
        //ScoreUtility.CalculateMeanAndSD();
        float mean = 8.545455f;
        float stdDev = 5.105498f;

        // Use CDF formula to get percentile rank
        return ScoreUtility.NormalDistributionCDF(score, mean, stdDev) * 100f;
    }

}
