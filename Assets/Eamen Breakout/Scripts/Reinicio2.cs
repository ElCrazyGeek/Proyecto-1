using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reinicio2 : MonoBehaviour
{
    public TMP_Text Puntaje;
    public int Score;
    public TMP_Text Vidas_txt;
    public int vidas;

    public GameObject Victoria;
    public GameObject Derrota;

    void Update()
    {
        Puntaje.text = Score.ToString();
        RevisarEstado(); // Llamamos a la función para comprobar si ganó o perdió
    }

    public void RestarVida(int cantidad)
    {
        vidas -= cantidad;
        Vidas_txt.text = vidas.ToString();
        Debug.Log("Vidas restantes: " + vidas);
    }

    public void AgregarPuntos(int puntos)
    {
        Score += puntos;
        Puntaje.text = Score.ToString();
        Debug.Log("Puntos actuales: " + Score);
    }

    void RevisarEstado()
    {
        if (Score >= 11) // Si llega a 11 puntos, gana
        {
            Debug.Log("Ganaste");
            Time.timeScale = 0; // Pausa el juego
            Victoria.SetActive(true); // Muestra el panel de victoria
        }

        if (vidas <= 0) // Si las vidas llegan a 0, pierde
        {
            Debug.Log("Perdiste");
            Time.timeScale = 0; // Pausa el juego
            Derrota.SetActive(true); // Muestra el panel de derrota
        }
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1; // Restablece el tiempo de juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarga la escena
    }
}
