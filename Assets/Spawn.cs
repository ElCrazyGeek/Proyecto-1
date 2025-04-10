using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
     public GameObject enemigo;

     public bool generando;

    void Start()
    {
      StartCoroutine(generador());//empezamos la corutina
    } 

    IEnumerator generador(){
        while(generando==true){
         // esto se ejecuta primero
   Debug.Log("Empieza la rutina");
    yield return new WaitForSeconds(1f);//esperamos 1 segundo 
    //esto se ejecuta despues 
    GameObject elEnemigo=Instantiate(enemigo,transform.position,Quaternion.identity); // despues de un segundo va instanciar el enemigo
    }
        }
   

    
    void Update()
    {
        
    }
}
