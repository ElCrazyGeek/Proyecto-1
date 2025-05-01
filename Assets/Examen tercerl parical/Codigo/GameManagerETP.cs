using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManagerETP : MonoBehaviour
{
    public static GameManagerETP Instance;

    public int score = 0;
    public int playerLives = 4;
    public float playerEnergy = 100f;
    private int enemiesAlive = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public Slider energyBar;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

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
        UpdateUI();
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemigo1").Length;
        Debug.Log($"Enemigos iniciales: {enemiesAlive}");
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    public void TakeDamage(float amount)
    {
        playerEnergy -= amount;
        UpdateUI();
        if (playerEnergy <= 0)
        {
            playerLives--;
            if (playerLives <= 0)
            {
                GameOver();
            }
            else
            {
                playerEnergy = 100f;
                RespawnPlayer();
            }
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;
        Debug.Log($"Enemigos restantes: {enemiesAlive}");
        if (enemiesAlive <= 0)
        {
            Victory();
        }
    }

    public void Victory()
    {
        Time.timeScale = 0f;
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No se puede activar victoryPanel porque no está asignado");
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No se puede activar gameOverPanel porque no está asignado");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        score = 0;
        playerLives = 4;
        playerEnergy = 100f;
        UpdateUI();
        SceneManager.LoadScene("Examen tercer parcial");
    }

    public void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Vidas: " + playerLives;
        if (energyBar != null) energyBar.value = playerEnergy;
    }

    public int GetScore() // Añadido para UIManager
    {
        return score;
    }

    private void RespawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Jugador");
        if (player != null)
        {
            player.transform.position = Vector3.zero; // Centro de la pantalla
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("No se encontró el Jugador para respawnear");
        }
    }
}