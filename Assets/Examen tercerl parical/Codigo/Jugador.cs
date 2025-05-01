using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject LaBalaPrefab;
    public Transform firePoint;
    public float LaBalaSpeed = 10f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        rb.linearVelocity = moveDirection * moveSpeed;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 aimDirection = (mousePos - firePoint.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        AimAndShoot(aimDirection);

        if (Input.GetKeyDown(KeyCode.K)) // Presiona K para probar
        {
            InstantDeath();
        }
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

    public void TakeDamage(float damage)
    {
        Debug.Log($"PlayerController.TakeDamage llamado con {damage} de daño");
        if (GameManagerETP.Instance != null)
        {
            GameManagerETP.Instance.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning("GameManagerETP no está configurado. No se aplicó daño.");
        }
    }

    public void InstantDeath()
    {
        Debug.Log("Jugador murió instantáneamente");
        if (GameManagerETP.Instance != null)
        {
            GameManagerETP.Instance.TakeDamage(100f);
        }
    }
}