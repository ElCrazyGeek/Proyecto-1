using UnityEngine;

public class LaBala : MonoBehaviour
{
    public float damage = 25f;
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
                enemigo.TakeDamage((int)damage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Jugador"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log($"Aplicando {damage} de daño al jugador");
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}