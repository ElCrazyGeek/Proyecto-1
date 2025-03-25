using UnityEngine;
using TMPro; // con esto importamos comandos de TextMeshPro

public class Pelota: MonoBehaviour
{ 
    public  int Score; //varibles numericas

    public TextMeshProUGUI textoP1; //variables de texto 

    Rigidbody2D PelotaRB;//Este es el rigitbody de 
    public float fuerza;

    public Transform Inicio;
    public GameObject Victoria, Derrota;

    public bool playing;

    void Start()
    {
       PelotaRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && playing==false){
            PelotaRB.AddForce(Vector2.one*fuerza,ForceMode2D.Impulse);
            playing=true;

        }
     
        if(Score>=5){
            Debug.Log("Ganaste");
            Time.timeScale=0; //congelamos el tiempo (el juego)
            Victoria.SetActive(true); // se activa el panel de Victoria
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Fail")){
           playing=false;
        PelotaRB.linearVelocity=Vector2.zero;
        transform.position=Inicio.position;
            textoP1.text="Perdiste" +Score.ToString();//cambiamos el texto
        }
        
        if(collision.gameObject.CompareTag("GolP1")){
           playing=false;
        PelotaRB.linearVelocity=Vector2.zero;
        transform.position=Inicio.position;
            Score++;
            textoP1.text="Player 1\n " +Score.ToString();//cambiamos el texto
        }
    }
     void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("bloque")){
            Destroy(collision.gameObject);
        }
    }
}
