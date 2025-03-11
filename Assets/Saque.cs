using UnityEngine;

public class Saque : MonoBehaviour
{ 
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
        playing=false;
        PelotaRB.linearVelocity=Vector2.zero;
        transform.position=Inicio.position;
    }
}
