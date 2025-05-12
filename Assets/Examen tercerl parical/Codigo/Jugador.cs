using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float fistDamage = 10f;
    public float batDamage = 25f;
    public float meleeRange = 1f;
    public float meleeAttackRate = 0.5f;
    public GameObject equippedWeapon;
    public GameObject pistolPickupPrefab;
    public GameObject batPickupPrefab;
    public LayerMask enemyLayer;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 movement;
    private float nextMeleeTime;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        nextMeleeTime = 0f;
        if (!gameObject.CompareTag("Jugador"))
        {
            Debug.LogWarning($"{gameObject.name}: Tag debe ser 'Jugador'");
        }
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 direction = (mousePos - transform.position).normalized;
        transform.right = direction;
        sr.flipY = direction.x < 0;

        if (equippedWeapon != null)
        {
            equippedWeapon.transform.right = direction;
            equippedWeapon.transform.localPosition = new Vector3(0.5f, 0f, 0f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (equippedWeapon == null)
            {
                MeleeAttack(false);
            }
            else if (equippedWeapon.CompareTag("Bat"))
            {
                MeleeAttack(true);
            }
        }

        if (Input.GetMouseButton(0) && equippedWeapon != null && equippedWeapon.CompareTag("PistolaPickup"))
        {
            Pistol pistol = equippedWeapon.GetComponent<Pistol>();
            if (pistol != null && pistol.CanShoot())
            {
                pistol.Shoot(direction);
                pistol.UpdateFireTime();
                Debug.Log("Jugador disparando");
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && equippedWeapon != null)
        {
            DropWeapon();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

    void MeleeAttack(bool useBat)
    {
        if (Time.time >= nextMeleeTime)
        {
            nextMeleeTime = Time.time + meleeAttackRate;
            Collider2D hit = Physics2D.OverlapCircle(transform.position, meleeRange, enemyLayer);
            if (hit != null && hit.CompareTag("Enemigo1"))
            {
                Enemy enemyScript = hit.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    float damage = useBat ? batDamage : fistDamage;
                    enemyScript.TakeDamage(damage, true);
                    Debug.Log($"Jugador golpeó a {hit.name} con {damage} de daño ({(useBat ? "bate" : "puños")})");
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (GameManagerETP.Instance != null)
        {
            GameManagerETP.Instance.TakeDamage(damage);
        }
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon);
            equippedWeapon = null;
        }

        equippedWeapon = Instantiate(weaponPrefab, transform);
        equippedWeapon.transform.localPosition = new Vector3(0.5f, 0f, 0f);
        equippedWeapon.transform.localRotation = Quaternion.identity;
        equippedWeapon.layer = gameObject.layer;
        CircleCollider2D collider = equippedWeapon.GetComponent<CircleCollider2D>();
        if (collider != null) Destroy(collider);
        Debug.Log($"Jugador equipó {(equippedWeapon.CompareTag("Bat") ? "bate" : "pistola")}");
    }

    void DropWeapon()
    {
        if (equippedWeapon == null)
        {
            Debug.LogWarning("No hay arma equipada para soltar");
            return;
        }

        GameObject pickupPrefab = equippedWeapon.CompareTag("Bat") ? batPickupPrefab : pistolPickupPrefab;
        GameObject weaponPrefab = equippedWeapon.CompareTag("Bat") ? batPickupPrefab.GetComponent<WeaponPickup>().weaponPrefab : pistolPickupPrefab.GetComponent<WeaponPickup>().weaponPrefab;
        if (pickupPrefab == null || weaponPrefab == null)
        {
            Debug.LogError($"No se asignó {(equippedWeapon.CompareTag("Bat") ? "batPickupPrefab" : "pistolPickupPrefab")} o weaponPrefab en PlayerController");
            Destroy(equippedWeapon);
            equippedWeapon = null;
            return;
        }

        GameObject pickup = Instantiate(pickupPrefab, transform.position + (Vector3)transform.right * 0.5f, Quaternion.identity);
        pickup.layer = LayerMask.NameToLayer("Pickups");

        Rigidbody2D rbPickup = pickup.GetComponent<Rigidbody2D>();
        if (rbPickup == null)
        {
            rbPickup = pickup.AddComponent<Rigidbody2D>();
            rbPickup.gravityScale = 0;
            rbPickup.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        rbPickup.linearVelocity = transform.right * 7f;

        CircleCollider2D collider = pickup.GetComponent<CircleCollider2D>();
        if (collider == null)
        {
            collider = pickup.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 0.5f;
        }

        WeaponPickup pickupScript = pickup.GetComponent<WeaponPickup>();
        if (pickupScript == null)
        {
            pickupScript = pickup.AddComponent<WeaponPickup>();
        }
        pickupScript.weaponPrefab = weaponPrefab;

        WeaponProjectile projectile = pickup.GetComponent<WeaponProjectile>();
        if (projectile == null)
        {
            projectile = pickup.AddComponent<WeaponProjectile>();
        }
        projectile.damage = equippedWeapon.CompareTag("Bat") ? 15f : 10f;
        projectile.sourceWeapon = weaponPrefab;

        Debug.Log($"Jugador lanzó {(equippedWeapon.CompareTag("Bat") ? "bate" : "pistola")} en {pickup.transform.position} con velocidad {rbPickup.linearVelocity}, weaponPrefab: {pickupScript.weaponPrefab}");

        Destroy(equippedWeapon);
        equippedWeapon = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}