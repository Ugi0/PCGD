using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] GameObject spawnArea;
    [SerializeField] float initialDistanceFromPlayer;
    [SerializeField] float varianceOnDistance;
    private float increasedDistance;

    private ObstacleSpawner obstacleSpawner;

    void Start() {
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
        Relocate();
    }
    void Relocate() {
        float colliderWidth = spawnArea.GetComponent<BoxCollider2D>().bounds.size.x;
        float colliderHeight = spawnArea.GetComponent<BoxCollider2D>().bounds.size.y;
        gameObject.transform.position = new Vector2(
            Random.Range(spawnArea.transform.position.x - colliderWidth, spawnArea.transform.position.x) - 
                varianceOnDistance/2 + Random.Range(0, varianceOnDistance) + initialDistanceFromPlayer, 
            Random.Range(spawnArea.transform.position.y - colliderHeight, spawnArea.transform.position.y)
        );

        if (obstacleSpawner != null)
        {
            obstacleSpawner.SpawnObstacles(); // Spawn new obstacles whenever the target is relocated
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("ThrowableObject")) {
            ScoreManager.instance.AddPoints(); // Add points to the score
            Relocate();
            ThrowScript.instance.ResetThrow(); //make new throwable object
            increasedDistance += 1;
        //}
    }
}