using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 10f;
    public float damage = 10f;
    public float lifetime = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError($"{gameObject.name}: Rigidbody2D no encontrado");
            Destroy(gameObject);
            return;
        }
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifetime);
        Debug.Log($"{gameObject.name}: Inicializada con dirección {direction}, velocidad {speed}, daño {damage}");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador"))
        {
            GameManagerETP.Instance.TakeDamage(damage);
            Debug.Log($"{gameObject.name}: Golpeó al jugador, daño {damage}");
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemigo1"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, false);
                Debug.Log($"{gameObject.name}: Golpeó a {collision.name}, daño {damage}");
            }
            Destroy(gameObject);
        }
    }
}