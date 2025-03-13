using UnityEngine;
using TMPro; // con esto importamos comandos de TextMeshPro

public class Saque : MonoBehaviour
{ 
    public  int ScoreP1; //varibles numericas

    public  int ScoreP2;
    public TextMeshProUGUI textoP1, textoP2; //variables de texto 

    Rigidbody2D PelotaRB;//Este es el rigitbody de 
    public float fuerza;

    public Transform Inicio;

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
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("GolP1")){
           playing=false;
        PelotaRB.linearVelocity=Vector2.zero;
        transform.position=Inicio.position;
            ScoreP1++;
            textoP1.text="Player 1\n " +ScoreP1.ToString();//cambiamos el texto
        }

         if(collision.gameObject.CompareTag("GolP2")){
            playing=false;
        PelotaRB.linearVelocity=Vector2.zero;
        transform.position=Inicio.position;
            ScoreP2++;
            textoP2.text="Player 2\n " +ScoreP2.ToString();//cambiamos el texto 
        }
        
    }
}
