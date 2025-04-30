using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject LaBalaPrefab;
    public Transform firePoint;
    public float LaBalaSpeed = 10f;

    public int maxHealth = 100;
    private int currentHealth;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Movimiento en 4 direcciones con WASD o flechas
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

        // Aplicar movimiento en FixedUpdate, pero almacenar dirección aquí
        rb.linearVelocity = moveDirection * moveSpeed;

        // Rotar el firePoint hacia el ratón
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 aimDirection = (mousePos - firePoint.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        // Disparar
        AimAndShoot(aimDirection);
    }

    void AimAndShoot(Vector2 aimDirection)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (LaBalaPrefab == null || firePoint == null)
            {
                Debug.LogError("LaBalaPrefab o firePoint no están asignados");
                return;
            }

            GameObject bullet = Instantiate(LaBalaPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb == null)
            {
                Debug.LogError("El prefab de la bala no tiene Rigidbody2D");
                Destroy(bullet);
                return;
            }
            bulletRb.linearVelocity = aimDirection * LaBalaSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Jugador recibió {damage} de daño. Vida actual: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void InstantDeath()
    {
        Debug.Log("Jugador murió instantáneamente");
        Die();
    }

    void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DefeatScreen");
    }
}