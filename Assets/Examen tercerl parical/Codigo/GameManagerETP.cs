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
    private int enemiesInAlertOrThreat = 0;

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
            DontDestroyOnLoad(gameObject);
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
        enemiesInAlertOrThreat = 0;
        Debug.Log($"Enemigos iniciales: {enemiesAlive}");
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SyncMusicWithGameState();
        }
        else
        {
            Debug.LogWarning("MusicManager no encontrado al iniciar GameManagerETP");
        }
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

    public void EnemyDied(bool isMainTarget = false)
    {
        enemiesAlive--;
        Debug.Log($"Enemigos restantes: {enemiesAlive}");
        if (isMainTarget)
        {
            Victory();
        }
        else
        {
            AddScore(100); // 100 puntos por enemigo opcional
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

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayPacificMusic();
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
        enemiesInAlertOrThreat = 0;
        UpdateUI();
        SceneManager.LoadScene("Examen tercer parcial");
    }

    public void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Vidas: " + playerLives;
        if (energyBar != null) energyBar.value = playerEnergy;
    }

    public int GetScore()
    {
        return score;
    }

    private void RespawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Jugador");
        if (player != null)
        {
            player.transform.position = Vector3.zero;
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("No se encontró el Jugador para respawnear");
        }
    }

    public void EnemyEnteredAlertOrThreat()
    {
        enemiesInAlertOrThreat++;
        Debug.Log($"Enemigos en Alerta/Amenaza: {enemiesInAlertOrThreat}");
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayAlertMusic();
        }
        else
        {
            Debug.LogWarning("MusicManager no encontrado");
        }
    }

    public void EnemyReturnedToPacific()
    {
        enemiesInAlertOrThreat = Mathf.Max(0, enemiesInAlertOrThreat - 1);
        Debug.Log($"Enemigos en Alerta/Amenaza: {enemiesInAlertOrThreat}");
        if (enemiesInAlertOrThreat == 0 && MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayPacificMusic();
        }
    }

    public int GetEnemiesInAlertOrThreat()
    {
        return enemiesInAlertOrThreat;
    }
}