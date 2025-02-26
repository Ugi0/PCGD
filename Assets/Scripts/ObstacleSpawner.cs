using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public static ObstacleSpawner instance;

    public GameObject obstaclePrefab; // Prefab to spawn
    public int startingLevel; //The number when the obstacles start spawning
    private float targetSpawnCount = 0; // Track the number of targets spawned

    public GameObject targetContainer;
    public GameObject[] targets;
    public GameObject targetSpawnArea;

    public BoxCollider2D groundCollider;
    public BoxCollider2D sidewalkCollider;
    public BoxCollider2D houseCollider;

    float MAX_DISTANCE = 15f;
    int HITS_UNTIL_MAX_DISTANCE = 15;
    private Vector2 originalSpawnPosition;

    [SerializeField] float initialDistanceFromPlayer = 2;

    private void Start()
    {
        originalSpawnPosition = targetSpawnArea.transform.position;
        SpawnTargets();
    }

    public float SpawnTargets()
    {
        BoxCollider2D spawnCollider = targetSpawnArea.GetComponent<BoxCollider2D>();
        if (spawnCollider == null)
        {
            Debug.LogError("Spawn area does not have a BoxCollider2D!");
            return 0;
        }

        // Move the spawn zone forward progressively

        float xCord = Mathf.Min(MAX_DISTANCE, initialDistanceFromPlayer + 
            (targetSpawnCount / HITS_UNTIL_MAX_DISTANCE * (MAX_DISTANCE - initialDistanceFromPlayer)));
        spawnCollider.size = new Vector2(xCord, 
            spawnCollider.size.y);

        Vector2 currentOffset = spawnCollider.offset;
        currentOffset.x = 0.5f + 0.5f * Mathf.Min(targetSpawnCount, HITS_UNTIL_MAX_DISTANCE); // Fix the offset of the left edge
        spawnCollider.offset = currentOffset;

        float minX = spawnCollider.bounds.min.x;
        float maxX = spawnCollider.bounds.max.x;
        float minY = spawnCollider.bounds.min.y;
        float maxY = spawnCollider.bounds.max.y;

        Vector2 spawnPosition = Vector2.zero;
        GameObject randomTarget = null;

        // Keep trying until a suitable target is found
        for (int i = 0; i < 10; i++) // Arbitrary number of attempts
        {
            // Take max of 2 random numbers to weight the spawn towards the end
            xCord = Mathf.Max(Random.Range(minX, maxX), Random.Range(minX, maxX));
            spawnPosition = new Vector2(xCord, Random.Range(minY, maxY));
            randomTarget = GetValidTarget(spawnPosition);
            if (randomTarget != null)
                break;
        }

        if (randomTarget != null)
        {
            Instantiate(randomTarget, spawnPosition, Quaternion.identity, targetContainer.transform);
        }
        else
        {
            Debug.LogWarning("No suitable target found to spawn!");
        }
        return xCord;
    }

    private GameObject GetValidTarget(Vector2 spawnPosition)
    {
        List<GameObject> validTargets = new List<GameObject>();

        foreach (GameObject targetObj in targets)
        {
            Target target = targetObj.GetComponent<Target>();
            if (target == null) continue;

            // Check if the target can spawn in this position
            if ((target.canSpawnHighway && groundCollider.bounds.Contains(spawnPosition)) ||
                (target.canSpawnSidewalk && sidewalkCollider.bounds.Contains(spawnPosition)) ||
                (target.canSpawnHouse && houseCollider.bounds.Contains(spawnPosition)))
            {
                validTargets.Add(targetObj);
            }
        }

        if (validTargets.Count > 0)
        {
            return validTargets[Random.Range(0, validTargets.Count)];
        }
        return null;
    }

    void Awake()
    {
        instance = this;
    }

    public void SpawnObstacles(float targetDistance)
    {

        targetSpawnCount++; // Increase the counter every time a new target spawns

        // Only spawn obstacles after the second target spawn (i.e., from the third spawn onward)
        if (targetSpawnCount < startingLevel)
        {
            return;
        }

        // Remove all previous obstacles before spawning new ones
        ClearOldObstacles();

        int obstacleCount = Random.Range(1, (int) (targetSpawnCount / 5) + 1); // Randomize between 1 and (rounds / 5) obstacles

        for (int i = 0; i < obstacleCount; i++)
        {
            SpawnObstacle(targetDistance);
        }
    }
    public void Reset() {
        targetSpawnCount = 0;
        ClearOldObstacles();
        ClearOldTargets();

        targetSpawnArea.transform.position = originalSpawnPosition;
        GameStateManager.instance.StartDelayedAction("SpawnTarget", 1f, () =>
        {
            SpawnTargets();
        });
        }

    void SpawnObstacle(float targetDistance)
    {
        BoxCollider2D spawnCollider = targetSpawnArea.GetComponent<BoxCollider2D>();

        float minX = spawnCollider.bounds.min.x;
        float maxX = spawnCollider.bounds.max.x;
        float minY = spawnCollider.bounds.min.y;
        float maxY = spawnCollider.bounds.max.y;

        Vector2 spawnPosition = new Vector2(
            Random.Range(minX, targetDistance),
            Random.Range(minY, maxY)
        );

        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity).tag = "Obstacle"; // Ensure the obstacle has the right tag
    }

    public void ClearOldObstacles()
    {
        GameObject[] oldObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in oldObstacles)
        {
            Destroy(obstacle);
        }
    }

    public void ClearOldTargets()
    {
        Target[] targets = FindObjectsOfType<Target>();

        foreach (Target obj in targets)
        {
            Destroy(obj.gameObject);
        }
    }
}
