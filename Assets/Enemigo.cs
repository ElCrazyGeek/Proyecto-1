using UnityEngine;

public class Enemigo : MonoBehaviour
{
   public Transform pelota;
   public float pelotaVel; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float target = Mathf.MoveTowards(transform.position.y, pelota.position.y, pelotaVel*Time.deltaTime);
        transform.position= new Vector2(transform.position.x, target);
    }
}
