using UnityEngine;

public class Balazo : MonoBehaviour
{
   GameManager gm;
    void Start()
    {
        //cuando parace la bala, busca el manager y lo asigna a su variable gm
        gm=GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemigo")){
            Destroy(collision.gameObject);
            gm.sumarPuntos();//ejecuto la funcion creada en el Game Manager 
        }
    }

    void Update()
    {
        
    }
}
