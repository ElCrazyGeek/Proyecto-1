using UnityEngine;

public class ThrownWeapon : MonoBehaviour
{
    public float throwDamage = 15f;
    public bool canKnockOut = true; // Armas arrojadas noquean
    public float throwSpeed = 10f;
    public Vector2 aimDirection;
    public bool isMeleeWeapon;
    public GameObject pistolPickupPrefab;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = aimDirection * throwSpeed;
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo1"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(throwDamage, canKnockOut);
                Debug.Log($"Arma arrojada golpeó a {collision.name}, daño: {throwDamage}");
            }
            if (!isMeleeWeapon && pistolPickupPrefab != null)
            {
                Instantiate(pistolPickupPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}