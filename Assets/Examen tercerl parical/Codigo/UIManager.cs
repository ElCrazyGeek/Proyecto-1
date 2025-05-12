using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI victoryScoreText;

    void Start()
    {
        UpdateScoreUI();
    }

    public void RestartGame()
    {
        if (GameManagerETP.Instance != null)
        {
            GameManagerETP.Instance.RestartGame();
        }
        else
        {
            Debug.LogWarning("GameManagerETP no está configurado. Reiniciando manualmente.");
            Time.timeScale = 1f;
            SceneManager.LoadScene("Examen tercer parcial");
        }
    }

    public void UpdateScoreUI()
    {
        if (GameManagerETP.Instance != null)
        {
            if (gameOverScoreText != null)
                gameOverScoreText.text = "Score: " + GameManagerETP.Instance.GetScore();
            if (victoryScoreText != null)
                victoryScoreText.text = "Score: " + GameManagerETP.Instance.GetScore();
        }
    }
}