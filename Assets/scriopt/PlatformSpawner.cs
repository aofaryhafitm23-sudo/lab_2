using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    public GameObject normalPlatformPrefab;
    public GameObject breakablePlatformPrefab;
    public GameObject bouncePlatformPrefab;
    public GameObject movingPlatformPrefab;

    [Header("Spawn Settings")]
    public int startPlatforms = 3;
    public float startY = -2f;
    public float minX = -2.2f;
    public float maxX = 2.2f;
    public float minYGap = 4f;
    public float maxYGap = 8f;

    [Header("Dynamic Spawn")]
    public float spawnAheadDistance = 10f;
    public float destroyDistanceBelowPlayer = 8f;

    [Header("Spawn Chances")]
    [Range(0f, 1f)] public float breakableChance = 0.15f;
    [Range(0f, 1f)] public float bounceChance = 0.12f;
    [Range(0f, 1f)] public float movingChance = 0.10f;

    private float highestSpawnY;
    private readonly List<GameObject> spawnedPlatforms = new List<GameObject>();

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player is not assigned!");
            return;
        }

        GenerateInitialPlatforms();
    }

    private void Update()
    {
        if (player == null) return;

        while (highestSpawnY < player.position.y + spawnAheadDistance)
        {
            SpawnPlatform();
        }

        CleanupOldPlatforms();
    }

    private void GenerateInitialPlatforms()
    {
        float currentY = startY;

        for (int i = 0; i < startPlatforms; i++)
        {
            float x = Random.Range(minX, maxX);
            GameObject prefabToSpawn = GetRandomPlatformPrefab(currentY);

            GameObject platform = Instantiate(prefabToSpawn, new Vector3(x, currentY, 0f), Quaternion.identity);
            platform.tag = "Platform";

            spawnedPlatforms.Add(platform);
            currentY += Random.Range(minYGap, maxYGap);
        }

        highestSpawnY = currentY;
    }

    private void SpawnPlatform()
    {
        float x = Random.Range(minX, maxX);
        GameObject prefabToSpawn = GetRandomPlatformPrefab(highestSpawnY);

        GameObject platform = Instantiate(prefabToSpawn, new Vector3(x, highestSpawnY, 0f), Quaternion.identity);
        platform.tag = "Platform";

        spawnedPlatforms.Add(platform);
        highestSpawnY += Random.Range(minYGap, maxYGap);
    }

    private GameObject GetRandomPlatformPrefab(float yLevel)
    {
        float roll = Random.value;

        // Самые первые платформы лучше делать обычными
        if (yLevel < 5f)
            return normalPlatformPrefab;

        if (roll < breakableChance && breakablePlatformPrefab != null)
            return breakablePlatformPrefab;

        if (roll < breakableChance + bounceChance && bouncePlatformPrefab != null)
            return bouncePlatformPrefab;

        if (roll < breakableChance + bounceChance + movingChance && movingPlatformPrefab != null)
            return movingPlatformPrefab;

        return normalPlatformPrefab;
    }

    private void CleanupOldPlatforms()
    {
        for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
        {
            if (spawnedPlatforms[i] == null)
            {
                spawnedPlatforms.RemoveAt(i);
                continue;
            }

            if (spawnedPlatforms[i].transform.position.y < player.position.y - destroyDistanceBelowPlayer)
            {
                Destroy(spawnedPlatforms[i]);
                spawnedPlatforms.RemoveAt(i);
            }
        }
    }
}