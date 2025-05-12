using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    public float damage = 10f;
    public GameObject sourceWeapon;
    private Rigidbody2D rb;
    private float lifetime = 3f;
    private bool hasHit;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError($"{gameObject.name}: Rigidbody2D no encontrado");
            return;
        }
        Invoke(nameof(StopProjectile), lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Enemigo1"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, false);
                StartCoroutine(enemy.KnockOut());
                Debug.Log($"{gameObject.name} golpeó a {other.name}, causando {damage} de daño y KO");
                hasHit = true;
                StopProjectile();
            }
        }
    }

    void StopProjectile()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
#pragma warning disable 0618
            rb.isKinematic = true;
#pragma warning restore 0618
        }
        Destroy(this); // Solo destruir el componente
        Debug.Log($"{gameObject.name} se detuvo y es ahora un pickup");
    }
}