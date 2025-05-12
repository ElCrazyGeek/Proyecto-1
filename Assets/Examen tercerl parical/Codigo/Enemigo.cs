using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Ataque, Amenaza, KnockedOut }

    public EnemyState currentState = EnemyState.Ataque;
    public bool isMainTarget = false;
    public float health = 50f;
    public float moveSpeed = 3f;
    public float visionAngle = 60f;
    public float visionDistance = 15f;
    public float threatDistance = 5f;
    public float meleeDamage = 10f;
    public float meleeRange = 2.5f; // Aumentado
    public float meleeAttackRate = 0.5f;
    public LayerMask playerLayer;
    public LayerMask pickupLayer;
    public GameObject equippedWeapon;
    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer sr;
    private float nextMeleeTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        player = playerController != null ? playerController.transform : null;
        if (player == null) Debug.LogError($"{gameObject.name}: Jugador no encontrado");
        nextMeleeTime = 0f;

        // Debug arma inicial
        if (equippedWeapon != null)
        {
            Debug.Log($"{gameObject.name}: Arma inicial equipada: {equippedWeapon.name}, tag: {equippedWeapon.tag}, tiene Pistol: {equippedWeapon.GetComponent<Pistol>() != null}");
        }
    }

    void Update()
    {
        if (currentState == EnemyState.KnockedOut || player == null) return;

        UpdateState();
        UpdateBehavior();

        if (equippedWeapon != null)
        {
            equippedWeapon.transform.localPosition = new Vector3(0.5f, 0f, 0f);
            equippedWeapon.transform.localRotation = Quaternion.identity;
            equippedWeapon.transform.right = transform.right;
            SpriteRenderer weaponSr = equippedWeapon.GetComponent<SpriteRenderer>();
            if (weaponSr != null)
            {
                weaponSr.sortingOrder = sr != null ? sr.sortingOrder + 1 : 1;
                Debug.Log($"{gameObject.name}: Arma posicionada en {equippedWeapon.transform.localPosition}, sortingOrder: {weaponSr.sortingOrder}");
            }
        }
    }

    void UpdateState()
    {
        Vector2 toPlayer = player.position - transform.position;
        float distanceToPlayer = toPlayer.magnitude;
        bool isPlayerInVisionCone = IsPlayerInVisionCone(toPlayer, distanceToPlayer);

        if (isPlayerInVisionCone && distanceToPlayer <= visionDistance)
        {
            currentState = equippedWeapon != null && equippedWeapon.CompareTag("PistolaPickup") ? EnemyState.Amenaza : EnemyState.Ataque;
            Debug.Log($"{gameObject.name}: Jugador detectado, estado: {currentState}");
        }
        else
        {
            GameObject nearestWeapon = FindNearestWeapon();
            if (nearestWeapon != null)
            {
                MoveTowards(nearestWeapon.transform.position);
                if (Vector2.Distance(transform.position, nearestWeapon.transform.position) < 0.5f)
                {
                    EquipWeapon(nearestWeapon);
                }
            }
        }
    }

    bool IsPlayerInVisionCone(Vector2 toPlayer, float distance)
    {
        if (distance > visionDistance) return false;

        Vector2 direction = toPlayer.normalized;
        float angle = Vector2.Angle(transform.right, direction);
        if (angle > visionAngle / 2f) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, playerLayer);
        bool canSeePlayer = hit.collider != null && hit.collider.CompareTag("Jugador");
        Debug.Log($"{gameObject.name} ve al jugador: {canSeePlayer}");
        return canSeePlayer;
    }

    void UpdateBehavior()
    {
        if (player != null && currentState != EnemyState.KnockedOut)
        {
            FaceTarget(player.position);
        }

        switch (currentState)
        {
            case EnemyState.Amenaza:
                if (equippedWeapon != null && equippedWeapon.CompareTag("PistolaPickup"))
                {
                    ShootAtPlayer();
                    rb.linearVelocity = Vector2.zero;
                }
                else
                {
                    MoveTowards(player.position);
                }
                break;

            case EnemyState.Ataque:
                if (equippedWeapon != null && equippedWeapon.CompareTag("PistolaPickup"))
                {
                    ShootAtPlayer();
                    rb.linearVelocity = Vector2.zero;
                }
                else
                {
                    if (Vector2.Distance(transform.position, player.position) <= meleeRange)
                    {
                        MeleeAttack();
                        rb.linearVelocity = Vector2.zero;
                    }
                    else
                    {
                        MoveTowards(player.position);
                    }
                }
                break;

            case EnemyState.KnockedOut:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    void FaceTarget(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.right = direction;
        if (sr != null)
        {
            sr.flipY = direction.x < 0;
        }
    }

    void MeleeAttack()
    {
        if (Time.time >= nextMeleeTime)
        {
            nextMeleeTime = Time.time + meleeAttackRate;
            Collider2D hit = Physics2D.OverlapCircle(transform.position, meleeRange, playerLayer);
            if (hit != null && hit.CompareTag("Jugador"))
            {
                float damage = equippedWeapon != null && equippedWeapon.CompareTag("Bat") ? 25f : meleeDamage;
                GameManagerETP.Instance.TakeDamage(damage);
                Debug.Log($"{gameObject.name} golpeó al jugador con {damage} de daño ({(equippedWeapon != null && equippedWeapon.CompareTag("Bat") ? "bate" : "puños")})");
            }
            else
            {
                Debug.Log($"{gameObject.name}: No se detectó jugador en rango (distancia: {Vector2.Distance(transform.position, player.position)}, meleeRange: {meleeRange}, playerLayer: {LayerMask.LayerToName(playerLayer.value)})");
            }
        }
    }

    void ShootAtPlayer()
    {
        if (equippedWeapon == null || !equippedWeapon.CompareTag("PistolaPickup"))
        {
            Debug.LogWarning($"{gameObject.name}: No tiene pistola equipada (equippedWeapon: {equippedWeapon})");
            return;
        }

        Pistol pistol = equippedWeapon.GetComponent<Pistol>();
        if (pistol == null)
        {
            Debug.LogError($"{gameObject.name}: Componente Pistol no encontrado en {equippedWeapon.name}");
            return;
        }

        if (pistol.CanShoot())
        {
            Vector2 aimDirection = (player.position - equippedWeapon.transform.position).normalized;
            pistol.Shoot(aimDirection);
            pistol.UpdateFireTime();
            Debug.Log($"{gameObject.name} disparando al jugador en dirección {aimDirection}, firePoint: {pistol.firePoint.position}");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Pistola no puede disparar");
        }
    }

    GameObject FindNearestWeapon()
    {
        Collider2D[] pickups = Physics2D.OverlapCircleAll(transform.position, visionDistance, pickupLayer);
        GameObject nearest = null;
        float minDistance = float.MaxValue;

        foreach (Collider2D pickup in pickups)
        {
            if (pickup.CompareTag("PistolaPickup") || pickup.CompareTag("BatPickup"))
            {
                float distance = Vector2.Distance(transform.position, pickup.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = pickup.gameObject;
                }
            }
        }
        Debug.Log($"{gameObject.name} encontró arma: {(nearest != null ? nearest.name : "ninguna")}");
        return nearest;
    }

    void EquipWeapon(GameObject weapon)
    {
        equippedWeapon = weapon;
        equippedWeapon.transform.SetParent(transform);
        equippedWeapon.transform.localPosition = new Vector3(0.5f, 0f, 0f);
        equippedWeapon.transform.localRotation = Quaternion.identity;
        equippedWeapon.layer = gameObject.layer;
        CircleCollider2D collider = weapon.GetComponent<CircleCollider2D>();
        if (collider != null) Destroy(collider);
        SpriteRenderer weaponSr = weapon.GetComponent<SpriteRenderer>();
        if (weaponSr != null)
        {
            weaponSr.sortingOrder = sr != null ? sr.sortingOrder + 1 : 1;
        }
        currentState = equippedWeapon.CompareTag("PistolaPickup") ? EnemyState.Amenaza : EnemyState.Ataque;
        Debug.Log($"{gameObject.name} equipó {weapon.name} (tag: {weapon.tag}, Pistol: {weapon.GetComponent<Pistol>() != null}) y cambió a {currentState}");
    }

    public void TakeDamage(float damage, bool canKnockOut)
    {
        health -= damage;
        if (sr != null)
        {
            StartCoroutine(FlashRed());
        }
        if (health <= 0)
        {
            if (equippedWeapon != null)
            {
                DropWeaponOnDeath();
            }
            if (GameManagerETP.Instance != null)
            {
                GameManagerETP.Instance.EnemyDied(isMainTarget);
            }
            Destroy(gameObject);
        }
        else if (canKnockOut)
        {
            StartCoroutine(KnockOut());
        }
    }

    void DropWeaponOnDeath()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        GameObject pickupPrefab = equippedWeapon.CompareTag("Bat") ? player.batPickupPrefab : player.pistolPickupPrefab;
        GameObject weaponPrefab = equippedWeapon.CompareTag("Bat") ? player.batPickupPrefab.GetComponent<WeaponPickup>().weaponPrefab : player.pistolPickupPrefab.GetComponent<WeaponPickup>().weaponPrefab;
        if (pickupPrefab != null && weaponPrefab != null)
        {
            GameObject pickup = Instantiate(pickupPrefab, transform.position + (Vector3)transform.right * 0.5f, Quaternion.identity);
            pickup.layer = LayerMask.NameToLayer("Pickups");
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
            Debug.Log($"{gameObject.name} soltó {(equippedWeapon.CompareTag("Bat") ? "bate" : "pistola")} al morir en {pickup.transform.position}");
        }
        else
        {
            Debug.LogError($"{gameObject.name}: No se encontró pickupPrefab o weaponPrefab para {(equippedWeapon.CompareTag("Bat") ? "bate" : "pistola")}");
        }
        Destroy(equippedWeapon);
        equippedWeapon = null;
    }

    IEnumerator FlashRed()
    {
        if (sr != null)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
        }
    }

    public IEnumerator KnockOut()
    {
        currentState = EnemyState.KnockedOut;
        rb.linearVelocity = Vector2.zero;
        if (equippedWeapon != null)
        {
            PlayerController player = FindFirstObjectByType<PlayerController>();
            GameObject pickupPrefab = equippedWeapon.CompareTag("Bat") ? player.batPickupPrefab : player.pistolPickupPrefab;
            GameObject weaponPrefab = equippedWeapon.CompareTag("Bat") ? player.batPickupPrefab.GetComponent<WeaponPickup>().weaponPrefab : player.pistolPickupPrefab.GetComponent<WeaponPickup>().weaponPrefab;
            if (pickupPrefab != null && weaponPrefab != null)
            {
                GameObject pickup = Instantiate(pickupPrefab, transform.position + (Vector3)transform.right * 0.5f, Quaternion.identity);
                pickup.layer = LayerMask.NameToLayer("Pickups");
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
                Debug.Log($"{gameObject.name} soltó {(equippedWeapon.CompareTag("Bat") ? "bate" : "pistola")} en KO en {pickup.transform.position}");
            }
            else
            {
                Debug.LogError($"{gameObject.name}: No se encontró pickupPrefab o weaponPrefab para {(equippedWeapon.CompareTag("Bat") ? "bate" : "pistola")}");
            }
            Destroy(equippedWeapon);
            equippedWeapon = null;
        }
        yield return new WaitForSeconds(5f);
        currentState = EnemyState.Ataque;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Quaternion leftRot = Quaternion.Euler(0, 0, visionAngle / 2f);
        Quaternion rightRot = Quaternion.Euler(0, 0, -visionAngle / 2f);
        Vector2 leftDir = leftRot * transform.right;
        Vector2 rightDir = rightRot * transform.right;
        Gizmos.DrawRay(transform.position, leftDir * visionDistance);
        Gizmos.DrawRay(transform.position, rightDir * visionDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}