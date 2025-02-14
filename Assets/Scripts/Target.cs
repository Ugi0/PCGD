using UnityEngine;

public class Target : MonoBehaviour
{
    float MAX_DISTANCE = 17f;
    int HITS_UNTIL_MAX_DISTANCE = 15;
    [SerializeField] GameObject spawnArea;
    [SerializeField] float initialDistanceFromPlayer;
    [SerializeField] float varianceOnDistance;
    private float hitCount = 0;

    private ObstacleSpawner obstacleSpawner;

    void Start() {
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
        Relocate();
    }
    public void Reset() {
        hitCount = 0;
        Relocate();
        obstacleSpawner.Reset();
    }
    public void Relocate() {
        gameObject.SetActive(true);
        float colliderWidth = spawnArea.GetComponent<BoxCollider2D>().bounds.size.x;
        float colliderHeight = spawnArea.GetComponent<BoxCollider2D>().bounds.size.y;
        gameObject.transform.position = new Vector2(
            Random.Range(spawnArea.transform.position.x - colliderWidth, spawnArea.transform.position.x) - 
                varianceOnDistance/2 + Random.Range(0, varianceOnDistance) + initialDistanceFromPlayer 
                + ((hitCount / HITS_UNTIL_MAX_DISTANCE) * (MAX_DISTANCE - initialDistanceFromPlayer)), 
            Random.Range(spawnArea.transform.position.y - colliderHeight, spawnArea.transform.position.y)
        );

        if (obstacleSpawner != null)
        {
            obstacleSpawner.SpawnObstacles(); // Spawn new obstacles whenever the target is relocated
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
        gameObject.SetActive(false);
        GameStateManager.instance.registerHit();
        GameStateManager.instance.StartDelayedAction("Target", 1f, () => {
            Relocate();
            hitCount += 1;
        });
    }
}