using UnityEngine;

public class Destruccion : MonoBehaviour
{
     public Reinicio2 Reinicio;
    void Start()
    {
        Reinicio=FindFirstObjectByType<Reinicio2>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("bloque")){
            Reinicio.AgregarPuntos(1);
            Destroy(collision.gameObject);
        }
    }

}
