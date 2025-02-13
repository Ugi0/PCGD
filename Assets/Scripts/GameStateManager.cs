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

    int score = 0;
    int highscore = 0;
    private Dictionary<string, Coroutine> activeDelays = new Dictionary<string, Coroutine>();

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
    }
    public void Reset() {
        Debug.Log("Resetting game state");
        ResetHealth();
        score = -10;
        AddPoints();
        // Fix this for when other objects are added
        Target target = GameObject.FindGameObjectsWithTag("CollisionObject")[0].GetComponent<Target>();
        // TODO 
        // There seems to be a bug where after the first hit, the target is not found with this method, causing the sign to not relocate and birds not to respawn
        if (target != null) {
            target.Relocate();
        }        
    }
    public void ResetHealth() {
        Health = 3;
    }
    public void ReduceHealth() {
        Health -= 1;
        if (Health == 0) {
            // Game over
            Reset();
        }
    }

    public void AddPoints() {
        score += 10;
        scoreText.text = score.ToString() + " POINTS";

        if (highscore < score) {
            PlayerPrefs.SetInt("highscore", score);
        }
    }

    private void StartMusic()
    {
        AudioManager.Instance.PlayMusic("Theme");
    }
    public void registerThrow() {
        StartDelayedAction("Throw", 1f, () => {
            ReduceHealth();
            Debug.Log(Health + " health remaining");
        });
    }
    public void registerHit() {
        StopDelayedAction("Throw");
        ResetHealth();
        GameObject.Find("Player").GetComponent<PlayerController>().showOldThrow(false);
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
}
