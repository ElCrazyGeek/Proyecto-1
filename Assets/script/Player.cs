using UnityEngine;

public class Player : MonoBehaviour
{ 
    Rigidbody2D PlayerRB; 

    public float vel = 10;

     void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
    }

     void Update()
    {
         float movY = Input.GetAxis("Vertical")*vel;
         PlayerRB.linearVelocityY=movY; //asignamos el input de valocidad en y
    }
}
