using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartLevel();
    }

    private void StartLevel()
    {
        AudioManager.Instance.PlayMusic("Theme");
    }
}
