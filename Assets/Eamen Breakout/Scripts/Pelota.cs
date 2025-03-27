using UnityEngine;
using TMPro; // con esto importamos comandos de TextMeshPro

public class Pelota: MonoBehaviour
{ 
    public  int Vidas; //varibles numericas
    
    public int Score;

    public Transform incio;

    public TextMeshProUGUI textoP1, Vida_txt; //variables de texto 

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
            textoP1.text="Perdiste" +Vidas.ToString();//cambiamos el texto
        }
        
    }
     void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("bloque")){
            Destroy(collision.gameObject);
        }

         if(collision.gameObject.CompareTag("Fail")){
           playing=false;
        PelotaRB.linearVelocity=Vector2.zero;
        transform.position=Inicio.position;
            Vidas--;
            Vida_txt.text=Vidas.ToString();//cambiamos el texto
            if(Vidas==0){
            Debug.Log("Perdiste");
            Time.timeScale=0; //congelamos el tiempo (el juego)
            Derrota.SetActive(true); // se activa el panel de Victoria
        }
        }
    }
}
