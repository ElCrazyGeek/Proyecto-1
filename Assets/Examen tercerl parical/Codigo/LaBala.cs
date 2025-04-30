using UnityEngine;

public class LaBala : MonoBehaviour
{
    public int damage = 50; 
    public float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Bala colisionó con: {other.gameObject.name} (Tag: {other.tag})");

        if (other.CompareTag("Enemigo1"))
        {
            Enemigo1 enemigo = other.GetComponent<Enemigo1>();
            if (enemigo != null)
            {
                enemigo.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Jugador"))
        {
            PlayerController jugador = other.GetComponent<PlayerController>();
            if (jugador != null)
            {
                jugador.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}