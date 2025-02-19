using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Singleton approach for easy global access
    public static GameManager Instance { get; private set; }

    // Observer events
    public UnityEvent<int> OnScoreChanged;    // Pass new score
    public UnityEvent OnGameOver;
    public UnityEvent OnGameRestart;
    public UnityEvent OnHardModeEntered;

    private int score;
    private bool gameOver = false;
    private bool isHardMode = false;
    private GameObject currentMap;
    [SerializeField] private GameObject hardMapPrefab;
    [SerializeField] private GameObject normalMapPrefab;

    public bool IsHardMode => isHardMode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize with normal map
        SpawnMap(false);
    }

    // Called by Player / Obstacle triggers
    public void AddScore(int amount)
    {
        if (gameOver) return;

        score += amount;
        OnScoreChanged.Invoke(score);
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            OnGameOver.Invoke();
            Debug.Log("Game Over");
        }
    }

    public void ToggleMode()
    {
        isHardMode = !isHardMode;  // Toggle the mode

        // Destroy current map
        if (currentMap != null)
        {
            Destroy(currentMap);
        }

        // Reset game state, just like in RestartGame
        score = 0;
        gameOver = false;
        OnScoreChanged.Invoke(score);

        // Spawn new map with toggled mode
        SpawnMap(isHardMode);

        // Notify about mode change and restart
        OnHardModeEntered?.Invoke();
        OnGameRestart.Invoke();
    }

    private void SpawnMap(bool isHardMode)
    {
        // Debug to check which map is being spawned
        Debug.Log($"Spawning map. IsHardMode: {isHardMode}");

        // Clear all existing obstacles
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        foreach (GameObject platform in platforms)
        {
            Destroy(platform);
        }

        // Ensure old map is destroyed
        if (currentMap != null)
        {
            Destroy(currentMap);
        }

        // Check if prefabs are assigned
        if (hardMapPrefab == null || normalMapPrefab == null)
        {
            Debug.LogError("Map prefabs not assigned in GameManager!");
            return;
        }

        // Spawn the appropriate map
        currentMap = Instantiate(isHardMode ? hardMapPrefab : normalMapPrefab);
    }

    public void RestartGame()
    {
        if (currentMap != null)
        {
            Destroy(currentMap);
        }
        score = 0;
        gameOver = false;
        OnScoreChanged.Invoke(score);
        SpawnMap(isHardMode);  // Maintain current mode
        OnGameRestart.Invoke();
    }
}
