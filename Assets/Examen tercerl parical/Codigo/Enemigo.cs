using UnityEngine;

public class Enemigo1 : MonoBehaviour
{
    public int health = 100;
    public int contactDamage = 100;
    public float moveSpeed = 3f;
    public Transform target;

    public GameObject balaPrefab;
    public Transform firePoint;
    public float tiempoEntreDisparos = 2f;
    public float velocidadBala = 7f;
    private float tiempoSiguienteDisparo;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Jugador").transform;
        tiempoSiguienteDisparo = Time.time + tiempoEntreDisparos;
    }

    void Update()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (Time.time >= tiempoSiguienteDisparo)
        {
            Disparar();
            tiempoSiguienteDisparo = Time.time + tiempoEntreDisparos;
        }
    }

    void Disparar()
{
    if (target == null) return;

    Vector2 direccion = (target.position - transform.position).normalized;
    GameObject bala = Instantiate(balaPrefab, firePoint.position, Quaternion.identity);
    bala.GetComponent<Rigidbody2D>().linearVelocity = direccion * velocidadBala;  // Cambio de velocity a linearVelocity
}

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

   void Die()
{
    Destroy(gameObject);
    try
    {
        if (GameManagerETP.Instance != null)
        {
            GameManagerETP.Instance.AddScore(100);
        }
    }
    catch
    {
        Debug.LogWarning("GameManagerETP no está configurado en la escena");
    }
}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            other.GetComponent<PlayerController>().InstantDeath();
        }
    }
}
