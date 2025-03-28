using UnityEngine;
using TMPro;

public class Pelota : MonoBehaviour
{
    public int Vidas;
    public int Score;

    public Transform Paleta; // La bola sigue a la paleta antes del saque
    public TextMeshProUGUI textoP1, Vida_txt;
    public GameObject Victoria, Derrota;

    private Rigidbody2D PelotaRB;
    public float fuerza;

    private bool enPaleta = true; // Controla si la bola está pegada a la paleta

    void Start()
    {
        PelotaRB = GetComponent<Rigidbody2D>();
        ResetBola(); // Iniciamos con la bola pegada a la paleta
    }

    void Update()
    {
        if (enPaleta)
        {
            // La bola sigue la paleta
            transform.position = new Vector2(Paleta.position.x, Paleta.position.y + 0.5f);

            // Si el jugador presiona "Espacio", lanzamos la bola
            if (Input.GetKeyDown(KeyCode.Space))
            {
                enPaleta = false;
                PelotaRB.linearVelocity = new Vector2(fuerza, fuerza); // Disparamos la bola
            }
        }

        // Si el jugador presiona "0", reinicia la bola
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ResetBola();
        }

        // Condición de victoria
        if (Score >= 5)
        {
            Debug.Log("Ganaste");
            Time.timeScale = 0;
            Victoria.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fail"))
        {
            ResetBola(); // Si la bola cae, la reiniciamos
            textoP1.text = "Perdiste " + Vidas.ToString();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bloque"))
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Fail"))
        {
            ResetBola();
            Vidas--;
            Vida_txt.text = Vidas.ToString();
            if (Vidas == 0)
            {
                Debug.Log("Perdiste");
                Time.timeScale = 0;
                Derrota.SetActive(true);
            }
        }
    }

    // Método para reiniciar la bola
    public void ResetBola()
    {
        enPaleta = true;
        PelotaRB.linearVelocity = Vector2.zero; // Detenemos la bola
        transform.position = new Vector2(Paleta.position.x, Paleta.position.y + 0.5f); // Pegamos la bola a la paleta
    }

    // Método para reiniciar con un botón
    public void BotonReiniciar()
    {
        ResetBola();
    }
}
