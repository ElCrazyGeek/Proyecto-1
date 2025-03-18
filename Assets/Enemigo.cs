using UnityEngine;

public class Enemigo : MonoBehaviour
{
   public Transform pelota;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position= new Vector2(transform.position.x,pelota.position.y);
    }
}
