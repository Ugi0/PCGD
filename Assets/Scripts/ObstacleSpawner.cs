using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // Prefab to spawn
    public Vector3 spawnArea = new Vector3(10f, 0, 10f); // Spawn range

    void Start()
    {
        int obstacleCount = Random.Range(1, 4); // Randomize between 1 and 3

        for (int i = 0; i < obstacleCount; i++)
        {
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(1f, 5f), // Random height
            Random.Range(-spawnArea.z, spawnArea.z)
        );

        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
    // Set color for the Gizmos box
    Gizmos.color = Color.green;

    // Draw a wireframe cube that represents the spawn area
    Gizmos.DrawWireCube(transform.position, spawnArea * 2); // Multiply by 2 to get full width/depth
    }
}

