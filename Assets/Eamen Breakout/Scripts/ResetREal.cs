using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1.0f; // Aseguramos que el tiempo está en velocidad normal
    }

    // Método público para reiniciar la escena
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
