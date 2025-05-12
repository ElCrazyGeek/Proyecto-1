using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;
    private bool playerInRange;

    void Start()
    {
        if (weaponPrefab == null)
        {
            Debug.LogError($"{gameObject.name}: weaponPrefab no asignado en el Inspector");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            playerInRange = true;
            Debug.Log($"{gameObject.name} en rango del jugador");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            playerInRange = false;
            Debug.Log($"{gameObject.name} fuera de rango del jugador");
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            GameObject playerObj = GameObject.FindWithTag("Jugador");
            if (playerObj == null)
            {
                Debug.LogError($"{gameObject.name}: Jugador con tag 'Jugador' no encontrado");
                return;
            }
            PlayerController player = playerObj.GetComponent<PlayerController>();
            if (player == null || weaponPrefab == null)
            {
                Debug.LogError($"{gameObject.name}: weaponPrefab no asignado o PlayerController no encontrado");
                return;
            }
            player.EquipWeapon(weaponPrefab);
            Debug.Log($"Jugador recogió {gameObject.name}");
            Destroy(gameObject);
        }
    }
}