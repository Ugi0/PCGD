using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // Prefab to spawn
    public Vector3 spawnArea = new Vector3(10f, 0, 10f); // Spawn range
    private int targetSpawnCount = 0; // Track the number of targets spawned

    public void SpawnObstacles()
    {
        targetSpawnCount++; // Increase the counter every time a new target spawns

        // Only spawn obstacles after the second target spawn (i.e., from the third spawn onward)
        if (targetSpawnCount < 3)
        {
            return;
        }

        // Remove all previous obstacles before spawning new ones
        ClearOldObstacles();

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

        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity).tag = "Obstacle"; // Ensure the obstacle has the right tag
    }

    void ClearOldObstacles()
    {
        GameObject[] oldObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in oldObstacles)
        {
            Destroy(obstacle);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnArea * 2);
    }
}
