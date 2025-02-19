using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverPanel;  // Reference to the GameOver panel
    [SerializeField] private TextMeshProUGUI modeText;  // Reference to mode text in GameOver panel

    private void Start()
    {
        // Subscribe to GameManager events
        GameManager.Instance.OnScoreChanged.AddListener(UpdateScore);
        GameManager.Instance.OnGameOver.AddListener(ShowGameOver);
        GameManager.Instance.OnGameRestart.AddListener(HideGameOver);
        GameManager.Instance.OnHardModeEntered.AddListener(UpdateModeText);

        // Set initial mode text
        modeText.text = "Press 'R' to restart\n\nPress 'Enter' for hard mode!";
    }

    private void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore.ToString();
    }

    private void ShowGameOver()
    {
        // Show the GameOverPanel when the game ends
        gameOverPanel.SetActive(true);
        UpdateModeText();  // Update text when game over panel shows
    }

    private void HideGameOver()
    {
        gameOverPanel.SetActive(false);
        scoreText.text = "";
    }

    private void UpdateModeText()
    {
        string currentMode = GameManager.Instance.IsHardMode ? "easy" : "hard";
        modeText.text = $"Press 'R' to restart\n\nPress 'Enter' for {currentMode} mode!";
    }
}
