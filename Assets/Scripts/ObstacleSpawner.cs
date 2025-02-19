using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnYPos = -3f; // or random range
    [SerializeField] private float spawnXPos = 10f;
    [SerializeField] private float xPosVariation = 1f;      // How much X position can vary
    [SerializeField] private float tallObstacleChance = 0.3f; // 30% chance for tall obstacle

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    private void SpawnObstacle()
    {
        // Add random variation to X position
        float randomX = spawnXPos + Random.Range(-xPosVariation, xPosVariation);
        Vector3 spawnPos = new Vector3(randomX, spawnYPos, 0);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        // Random chance for tall obstacle
        if (Random.value < tallObstacleChance)
        {
            Vector3 topPos = spawnPos + new Vector3(0, 1f, 0);
            GameObject topObstacle = Instantiate(obstaclePrefab, topPos, Quaternion.identity);

            // Find and disable the ScoreZone on the top obstacle
            Transform scoreZone = topObstacle.transform.Find("ScoreZone");
            if (scoreZone != null)
            {
                scoreZone.gameObject.SetActive(false);
            }
        }
    }
}
