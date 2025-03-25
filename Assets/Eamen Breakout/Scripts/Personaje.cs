using UnityEngine;

public class Personaje : MonoBehaviour
{ 
    Rigidbody2D PersonajeRB; 

    public float vel = 10;

     void Start()
    {
        PersonajeRB = GetComponent<Rigidbody2D>();
    }

     void Update()
    {
         float movX = Input.GetAxis("Horizontal")*vel;
         PersonajeRB.linearVelocityX=movX; //asignamos el input de valocidad en X
    }
}