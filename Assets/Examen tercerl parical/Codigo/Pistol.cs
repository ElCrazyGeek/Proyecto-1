using UnityEngine;

public class Pistol : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float bulletSpeed = 10f;
    public float bulletDamage = 10f;
    public float nextFireTime;

    void Start()
    {
        if (bulletPrefab == null) Debug.LogError($"{gameObject.name}: bulletPrefab no asignado");
        if (firePoint == null) Debug.LogError($"{gameObject.name}: firePoint no asignado");
        nextFireTime = 0f;
    }

    public bool CanShoot()
    {
        bool canShoot = Time.time >= nextFireTime && bulletPrefab != null && firePoint != null;
        Debug.Log($"{gameObject.name}: CanShoot = {canShoot}, Time: {Time.time}, nextFireTime: {nextFireTime}");
        return canShoot;
    }

    public void Shoot(Vector2 direction)
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError($"{gameObject.name}: No se puede disparar, bulletPrefab o firePoint no asignados");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.direction = direction;
            bulletScript.damage = bulletDamage;
            bulletScript.speed = bulletSpeed;
            Debug.Log($"{gameObject.name}: Disparó bala con dirección {direction}, daño {bulletDamage}, velocidad {bulletSpeed}");
        }
        else
        {
            Debug.LogError($"{gameObject.name}: Bullet prefab no tiene componente Bullet");
        }
    }

    public void UpdateFireTime()
    {
        nextFireTime = Time.time + fireRate;
        Debug.Log($"{gameObject.name}: nextFireTime actualizado a {nextFireTime}");
    }
}