using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    
    private int Health = 3;

    public Text scoreText;
    public Text highscoreText;

    public GameObject healthSpritePrefab;
    public Transform healthContainer;
    public float healthSpacing = 20f;
    public float leftPadding = 20f;

    int score = 0;
    int highscore = 0;
    private Dictionary<string, Coroutine> activeDelays = new Dictionary<string, Coroutine>();
    private List<GameObject> rockIcons = new List<GameObject>();


    private void Awake() {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString() + " POINTS";
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();

        StartMusic();
        ResetHealth();
    }
    public void Reset() {
        Debug.Log("Resetting game state");
        ResetHealth();
        score = -1;
        AddPoints();
        UpdateRockDisplay(Health);
        ObstacleSpawner.instance.Reset();
    }
    public void ResetHealth() {
        Health = 3;
        UpdateRockDisplay(Health);
    }
    public void ReduceHealth() {
        Health -= 1;
        if (Health <= 0) {
            // Game over
            Reset();
        }
        UpdateRockDisplay(Health);
    }

    public void AddPoints() {
        score += 1;
        scoreText.text = score.ToString() + " POINTS";

        if (highscore < score) {
            PlayerPrefs.SetInt("highscore", score);
            highscoreText.text = "HIGHSCORE: " + score.ToString();
        }
    }

    private void StartMusic()
    {
        AudioManager.Instance.PlayMusic("Theme");
    }
    public void registerThrow() {
        StartDelayedAction("Throw", 2f, () => {
            ReduceHealth();
            Debug.Log(Health + " health remaining");
        });
    }
    public void registerHit() {
        StopDelayedAction("Throw");
        ResetHealth();
        PlayerController.instance.showOldThrow(false);
        PlayerController.instance.StartSkating();
    }

    public void StartTransition()
    {
        StartDelayedAction("StartSkating", .5f, () => {
            PlayerController.instance.AnimateSkateboard(true);
            BackgroundManager.instance.SkatingTransition(true);
            ObstacleSpawner.instance.ClearOldObstacles();
           StopTransition();
        });
    }

    public void StopTransition()
    {
        StartDelayedAction("StopSkating", 1f, () => {
            PlayerController.instance.AnimateSkateboard(false);
            PlayerController.instance.StopSkating();
            ObstacleSpawner.instance.SpawnTargets();
            ObstacleSpawner.instance.SpawnObstacles();
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
}
