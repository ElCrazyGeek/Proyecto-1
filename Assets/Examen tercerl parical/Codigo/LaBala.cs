using UnityEngine;

public class LaBala : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifetime = 2f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, lifetime); // Destruir tras lifetime segundos
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo1"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, false); // Balas no noquean
                Debug.Log($"Bala golpeó a {collision.name}, daño: {damage}");
            }
            Destroy(gameObject); // Destruir la bala al impactar
        }
        else if (collision.CompareTag("Jugador"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log($"Bala golpeó al jugador, daño: {damage}");
            }
            Destroy(gameObject);
        }
    }
}