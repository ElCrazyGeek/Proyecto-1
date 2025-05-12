using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public AudioSource musicSource;
    private float originalVolume;
    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        if (musicSource != null)
        {
            originalVolume = musicSource.volume;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        if (musicSource != null)
        {
            musicSource.volume = originalVolume;
        }
        isPaused = false;
        Debug.Log("Juego reanudado");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Escena reiniciada");
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        if (musicSource != null)
        {
            musicSource.volume = 0.2f;
        }
        isPaused = true;
        Debug.Log("Juego pausado");
    }
}