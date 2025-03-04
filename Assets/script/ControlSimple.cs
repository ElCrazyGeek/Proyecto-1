using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ControlSimple : MonoBehaviour
{
     Rigidbody2D PlayerRB;
     public float velocidad=1;
     
     public float fuerza = 10;

     public GameObject JumpScare; // declaro un gameobject
     public GameObject Victoria;
     public Transform inicio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    PlayerRB=GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        float ejeX=Input.GetAxis("Horizontal");
        PlayerRB.linearVelocityX=ejeX*velocidad; //velocidad de X de acuerdo al input

    //float ejeY=Input.GetAxis("Vertical");
    // PlayerRB.linearVelocityY=ejeY*velocidad;

     if (Input.GetKeyDown(KeyCode.Space)){
        PlayerRB.AddForce(Vector2.up*fuerza,ForceMode2D.Impulse);
    
     }
    }

    void OnCollisionEnter2D(Collision2D col){

        Debug.Log(col.gameObject.name);  //imprime el nombre dle objeto que choque
        if(col.gameObject.CompareTag("item")){//Destruye el objeto si es un item
        Destroy(col.gameObject); 
        }
        if(col.gameObject.CompareTag("Respawn")){
            JumpScare.SetActive(true); // activa el jumpscare
        }
         if(col.gameObject.CompareTag("Victoria")){
            Victoria.SetActive(true);
        }
        if(col.gameObject.CompareTag("Muerte definitiva")){
            Destroy(gameObject);
        }
    }
        void OnTriggerEnter2D(Collider2D col){

        Debug.Log(col.gameObject.name);  //imprime el nombre dle objeto que choque
        if(col.gameObject.CompareTag("muerte")){//Destruye el objeto si es un item
        transform.position=inicio.position; //l tocar l muerte regresas al punto 00
        }
       
    }

    
}
