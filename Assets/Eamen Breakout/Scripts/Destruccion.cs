using UnityEngine;

public class Destruccion : MonoBehaviour
{
 
    void OnColisionEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("bloque")){
            Destroy(collision.gameObject);
        }
    }

}
