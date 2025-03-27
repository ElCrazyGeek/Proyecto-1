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
    
     public void RestasVida(int Vidas){
       vidas -= Vidas;
       Debug.Log("Agregar puntos");
       }
    public void AgregarPuntos(int puntos){
       Score+=puntos;
       Debug.Log("Agregar puntos");
     }
    public void Awake()
    {   
        
        Time.timeScale=1.0f;//decongelamos el tiempo 
    }

    void Update()
    {
        Puntaje.text=Score.ToString();
        resetmalo();
        
    }
    public void resetmalo()
    {
    
        if(Score==11)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Cargando escena");
        }
         
        if(Score>=11){
            Debug.Log("Ganaste");
            Time.timeScale=0; //congelamos el tiempo (el juego)
            Victoria.SetActive(true); // se activa el panel de derrota
        }
        if(vidas==0){
            Debug.Log("Perdiste");
            Time.timeScale=-1; //congelamos el tiempo (el juego)
            Derrota.SetActive(true); // se activa el panel de Victoria
        }
   }
}
