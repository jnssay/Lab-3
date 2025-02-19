using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardModeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float initialDelay = 1f;
    [SerializeField] private float spawnYPos = -3f;
    [SerializeField] private float spawnXPos = 10f;
    [SerializeField] private float platformXOffset = 3f;
    [SerializeField] private float platformYOffset = 2f;
    [SerializeField] private float obstacleYOffset = 0.5f;    // Reduced default value, adjust in inspector
    [SerializeField] private float obstacleRandomYRange = 0.2f; // Optional: Add some randomness to Y position

    // Three distinct platform lengths
    [SerializeField] private float shortPlatformLength = 2f;
    [SerializeField] private float mediumPlatformLength = 3.5f;
    [SerializeField] private float longPlatformLength = 5f;

    // Three corresponding spawn intervals
    [SerializeField] private float shortPlatformInterval = 0.6f;
    [SerializeField] private float mediumPlatformInterval = 0.8f;
    [SerializeField] private float longPlatformInterval = 1f;

    private float timer;
    private float nextSpawnInterval;
    private bool initialPlatformSpawned = false;

    private void Start()
    {
        timer = 0f;
        nextSpawnInterval = initialDelay;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!initialPlatformSpawned && timer >= initialDelay)
        {
            SpawnPlatform();
            initialPlatformSpawned = true;
            timer = 0f;
            return;
        }

        if (initialPlatformSpawned && timer >= nextSpawnInterval)
        {
            SpawnPlatform();
            timer = 0f;
        }
    }

    private void SpawnPlatform()
    {
        float randomY = spawnYPos + platformYOffset + Random.Range(-0.5f, 0.5f);
        Vector3 spawnPos = new Vector3(spawnXPos + platformXOffset, randomY, 0);

        int sizeChoice = Random.Range(0, 3);
        float platformLength;

        switch (sizeChoice)
        {
            case 0: // Short
                platformLength = shortPlatformLength;
                nextSpawnInterval = shortPlatformInterval;
                SpawnPlatformWithObstacles(spawnPos, platformLength, false);
                break;
            case 1: // Medium
                platformLength = mediumPlatformLength;
                nextSpawnInterval = mediumPlatformInterval;
                SpawnPlatformWithObstacles(spawnPos, platformLength, true);
                break;
            default: // Long
                platformLength = longPlatformLength;
                nextSpawnInterval = longPlatformInterval;
                SpawnPlatformWithObstacles(spawnPos, platformLength, true);
                break;
        }

        Debug.Log($"Spawned {sizeChoice switch { 0 => "short", 1 => "medium", _ => "long" }} platform, next spawn in {nextSpawnInterval}s");
    }

    private void SpawnPlatformWithObstacles(Vector3 spawnPos, float platformLength, bool canHaveObstacles)
    {
        // Spawn the platform
        GameObject platform = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
        platform.transform.localScale = new Vector3(platformLength, 1f, 1f);

        // Only medium and long platforms can have obstacles
        if (canHaveObstacles && Random.value < 0.7f) // 70% chance for obstacles
        {
            int patternChoice = Random.Range(0, 4); // 4 different patterns
            float obstacleWidth = 1f; // Assuming obstacle width is 1 unit
            Vector3 baseObstaclePos = spawnPos + new Vector3(0, obstacleYOffset, 0);
            Vector3 upperObstaclePos = spawnPos + new Vector3(0, obstacleYOffset + 1f, 0);

            switch (patternChoice)
            {
                case 0: // Two obstacles side by side (touching)
                    SpawnObstacleWithoutScore(baseObstaclePos, -obstacleWidth / 2);
                    SpawnObstacleWithoutScore(baseObstaclePos, obstacleWidth / 2);
                    break;

                case 1: // Two stacked obstacles (touching side by side)
                    SpawnObstacleWithoutScore(baseObstaclePos, -obstacleWidth / 2);
                    SpawnObstacleWithoutScore(baseObstaclePos, obstacleWidth / 2);
                    SpawnObstacleWithoutScore(upperObstaclePos, -obstacleWidth / 2);
                    SpawnObstacleWithoutScore(upperObstaclePos, obstacleWidth / 2);
                    break;

                case 2: // Left side stacked, right side single
                    SpawnObstacleWithoutScore(baseObstaclePos, -obstacleWidth / 2);
                    SpawnObstacleWithoutScore(baseObstaclePos, obstacleWidth / 2);
                    SpawnObstacleWithoutScore(upperObstaclePos, -obstacleWidth / 2);
                    break;

                case 3: // Left side single, right side stacked
                    SpawnObstacleWithoutScore(baseObstaclePos, -obstacleWidth / 2);
                    SpawnObstacleWithoutScore(baseObstaclePos, obstacleWidth / 2);
                    SpawnObstacleWithoutScore(upperObstaclePos, obstacleWidth / 2);
                    break;
            }
        }
    }

    private void SpawnObstacleWithoutScore(Vector3 basePos, float xOffset)
    {
        float randomYVariation = Random.Range(-obstacleRandomYRange, obstacleRandomYRange);
        Vector3 obstaclePos = basePos + new Vector3(xOffset, randomYVariation, 0);
        GameObject obstacle = Instantiate(obstaclePrefab, obstaclePos, Quaternion.identity);

        // Find and disable the ScoreZone
        Transform scoreZone = obstacle.transform.Find("ScoreZone");
        if (scoreZone != null)
        {
            scoreZone.gameObject.SetActive(false);
        }
    }
}
