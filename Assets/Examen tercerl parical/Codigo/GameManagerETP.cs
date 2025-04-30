using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerETP : MonoBehaviour
{
    public static GameManagerETP Instance;

    public int score = 0;
    public int playerLives = 4; // Empieza con 4 porque si te disparan 4 veces es muerte

    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        playerLives -= amount;
        UpdateUI();

        if (playerLives <= 0)
        {
            GameOver();
        }
    }

    public void Victory()
    {
        Time.timeScale = 0f; // Pausa el juego
        victoryPanel.SetActive(true);
    }

    public void GameOver()
    {
        Time.timeScale = 0f; // Pausa el juego
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reinicia el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Vidas: " + playerLives;
    }
}
